using System;
using System.Linq;
using Newtonsoft.Json;
using System.Net;
using CMcG.CommonwealthBank.Data;
using System.Collections.Generic;

namespace CMcG.CommonwealthBank.Logic
{
    public class DataRetriever
    {
        public AppStatus        Status   { get; set; }
        public Action           Callback { get; set; }

        public LoginDetails GetLogonDetails()
        {
            using (var store = new DataStoreContext())
                return store.LoginDetails.FirstOrDefault();
        }

        public bool LoadData()
        {
            var loginDetails = GetLogonDetails();
            if (loginDetails == null)
                return false;

            var client   = new CookieAwareWebClient { BaseAddress = "https://www2.my.commbank.com.au" };
            var password = new TwoWayEncryption().Decrypt(loginDetails.Password);
            Login(client, loginDetails.Username, password);
            return true;
        }

        void Login(WebClient client, string username, string password)
        {
            Status.SetAction("Logging on...");
            var parameters = new JsonParameters
            {
                Params = new[]
                {
                    new NameValue("Request",  "login"),
                    new NameValue("UserName", username),
                    new NameValue("Password", password),
                    new NameValue("Token",    "")
                }
            };
            string paramRequest = JsonConvert.SerializeObject(parameters);

            client.Headers["Content-Type"] = "application/json";
            client.UploadStringCompleted += OnLoginCompleted;
            client.UploadStringAsync(new Uri("mobile/i/AjaxCalls.aspx", UriKind.Relative), "POST", paramRequest, "login");
        }

        void OnLoginCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            var client = (WebClient)sender;
            client.UploadStringCompleted -= OnLoginCompleted;
            if (e.Error is WebException)
            {
                Status.SetAction("Cannot find the server", true);
                Callback();
                return;
            }

            var msg      = e.Result.Substring(2, e.Result.Length - 4);
            var hasError = Newtonsoft.Json.Linq.JObject.Parse(msg)["ErrorMessages"].Any();

            if (hasError)
            {
                OnLogonFailed();
                return;
            }

            UpdateAccounts(msg);

            using (var store = new DataStoreContext())
            {
                if (store.Options.Any())
                    GetTransactions(client, store.CurrentOptions.SelectedAccountId);
                else
                {
                    Status.SetAction("Account not set.", true);
                    Callback();
                }
            }
        }

        void GetTransactions(WebClient client, int accountId)
        {
            Status.SetAction("Looking for new transactions...");
            var parameters = new JsonParameters
            {
                Params = new[]
                {
                    new NameValue("Request",         "getTransactions"),
                    new NameValue("AccountId",       accountId),
                    new NameValue("AccountIdIsUser", "true"),
                }
            };
            string paramRequest = JsonConvert.SerializeObject(parameters);

            client.Headers["Content-Type"] = "application/json";
            client.UploadStringCompleted += OnGetTransactionsCompleted;
            client.UploadStringAsync(new Uri("mobile/i/AjaxCalls.aspx", UriKind.Relative), "POST", paramRequest);
        }

        void OnGetTransactionsCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            var client = (WebClient)sender;
            client.UploadStringCompleted -= OnGetTransactionsCompleted;

            if (e.Error is WebException)
            {
                Status.SetAction("Cannot find the server", true);
                Callback();
                return;
            }

            var msg = e.Result.Substring(2, e.Result.Length - 4);

            UpdateTransactions(msg);
            Callback();
        }

        void OnLogonFailed()
        {
            using (var store = new DataStoreContext())
            {
                store.LoginDetails.DeleteAllOnSubmit(store.LoginDetails);
                store.SubmitChanges();
            }
            Status.SetAction("Error occured while trying to log on", true);
            Callback();
        }

        void UpdateAccounts(string message)
        {
            var result = JsonConvert.DeserializeObject<LoginResult>(message).AccountGroups;

            using (var store = new DataStoreContext())
            {
                foreach (var account in result.SelectMany(x => x.ListAccount))
                {
                    account.LastUpdate = DateTime.Now;
                    var original = store.Accounts.FirstOrDefault(x => x.AccountNumber == account.AccountNumber);
                    if (original == null)
                        store.Accounts.InsertOnSubmit(account);
                    else
                    {
                        original.Balance        = account.Balance;
                        original.AvailableFunds = account.AvailableFunds;
                        original.LastUpdate     = account.LastUpdate;
                    }
                }

                store.SubmitChanges();
            }
        }

        void UpdateTransactions(string message)
        {
            var result = JsonConvert.DeserializeObject<Account>(message);

            using (var store = new DataStoreContext())
            {
                var account      = store.Accounts.FirstOrDefault(x => x.AccountNumber == result.AccountNumber);
                var toDelete     = account.TransactionList.Where(x => !AlreadyExists(result .Transactions,    x)).ToArray();
                var transactions = result .Transactions   .Where(x => !AlreadyExists(account.TransactionList, x)).ToArray();

                store.Transactions.DeleteAllOnSubmit(toDelete);

                foreach (var transaction in transactions.Reverse())
                    account.TransactionList.Add(transaction);

                store.SubmitChanges();
                Status.SetAction(transactions.Count() + " new transactions found.", true);
            }
        }

        bool AlreadyExists(IEnumerable<Transaction> list, Transaction transaction)
        {
            return list.Any(y => y.Equals(transaction));
        }
    }
}
