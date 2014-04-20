using System;
using System.Linq;
using Newtonsoft.Json;
using System.Net;
using CMcG.Bankr.Data;
using System.Collections.Generic;
using Windows.UI.Notifications;

namespace CMcG.Bankr.Logic.Queries
{
    public class GetTransactionsQuery : CommBankQuery
    {
        int m_accountId;

        protected override bool CanRun()
        {
            using (var store = new DataStoreContext())
            {
                bool hasAccount = store.Options.Any();
                if (hasAccount)
                    m_accountId = store.CurrentOptions.SelectedAccountId;
                else
                    Status.SetAction("Account not set.", true);

                return hasAccount;
            }
        }

        protected override string Action
        {
            get { return "Looking for new transactions..."; }
        }

        protected override NameValue[] Parameters
        {
            get
            {
                return new[]
                {
                    new NameValue("Request",         "getTransactions"),
                    new NameValue("AccountId",       m_accountId),
                    new NameValue("AccountIdIsUser", "true"),
                };
            }
        }

        protected override void ProcessResult(string response, bool hasError)
        {
            var result = JsonConvert.DeserializeObject<Account>(response);

            using (var store = new DataStoreContext())
            {
                var account      = store.Accounts.FirstOrDefault(x => x.AccountNumber == result.AccountNumber);
                var toDelete     = account.TransactionList.Where(x => !AlreadyExists(result .Transactions,    x)).ToArray();
                var transactions = result .Transactions   .Where(x => !AlreadyExists(account.TransactionList, x)).ToArray();

                store.Transactions.DeleteAllOnSubmit(toDelete);

                foreach (var transaction in transactions.Reverse())
                    account.TransactionList.Add(transaction);

                store.SubmitChanges();
                var action = transactions.Length + " new transactions found.";
                string more;
                if (transactions.Length == 1)
                    more = string.Format("${0:0.00} {1}", transactions[0].AbsAmount, transactions[0].Summary);
                else
                {
                    decimal amount = transactions.Sum(x => x.Amount);
                    more = string.Format("Totalling ${0:0.00} {1}.", Math.Abs(amount), amount > 0 ? "in" : "out");
                }
                Status.SetImportantAction(action, more, true);
                var template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            }
        }

        bool AlreadyExists(IEnumerable<Transaction> list, Transaction transaction)
        {
            return list.Any(y => y.Equals(transaction));
        }
    }
}
