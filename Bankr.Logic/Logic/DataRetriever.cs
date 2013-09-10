using System;
using System.Linq;
using System.Net.Http;
using CMcG.Bankr.Data;
using System.Threading.Tasks;
using CMcG.Bankr.Logic.Queries;

namespace CMcG.Bankr.Logic
{
    public class DataRetriever
    {
        const string Url = "https://www.my.commbank.com.au/mobile/i/default.aspx";

        public AppStatus Status   { get; set; }
        public Action    Callback { get; set; }

        string m_sessionId;

        public bool CanLoadData
        {
            get { return LogonQuery.GetLogonDetails() != null; }
        }

        static Uri m_uri;
        async Task<Uri> GetUri()
        {
            if (m_uri != null)
                return m_uri;

            var findResult = await new HttpClient().GetAsync(Url);
            m_uri = new Uri("https://" + findResult.RequestMessage.RequestUri.Host);
            return m_uri;
        }

        public async Task<HttpClient> Login()
        {
            var uri    = await GetUri();
            var client = new HttpClient { BaseAddress = uri };

            m_sessionId = await RunQuery<LogonQuery>(client);
            return client;
        }

        public async void LoadData()
        {
            var client = await Login();
            if (m_sessionId != null)
            {
                await RunQuery<GetUpcomingTransactionsQuery>(client);
                await RunQuery<GetTransactionsQuery>(client);
            }
            Callback();
        }

        public bool CanLoadTransferAccounts
        {
            get { return LogonQuery.GetLogonDetails() != null; }
        }

        public async void LoadTransferAccounts()
        {
            var client = await Login();
            if (m_sessionId != null)
            {
                await RunQuery<GetTransferAccountsQuery>(client);
            }
            Callback();
        }

        Task<string> RunQuery<T>(HttpClient client) where T : CommBankQuery, new()
        {
            return new T
            {
                Status    = Status,
                SessionId = m_sessionId
            }.Start(client);
        }

        public bool CanTransferMoney
        {
            get { return LogonQuery.GetLogonDetails() != null; }
        }

        public async Task<string> TransferMoney(int fromAccount, int toAccount, string description, decimal amount)
        {
            var client = await Login();
            if (m_sessionId == null)
                return null;

            await RunQuery<GetTransferAccountsQuery>(client);

            using (var store = new DataStoreContext())
            {
                var fromData     = store.Accounts.First(x => x.Id == fromAccount);
                var fromSendData = store.TransferToAccounts.First(x => x.Id == fromAccount && x.AccountName == fromData.AccountName);
                fromAccount      = fromSendData.SenderId ?? -1;
            }

            var query = new TransferMoneyQuery
            {
                Status        = Status,
                SessionId     = m_sessionId,
                FromAccountId = fromAccount,
                ToAccountId   = toAccount,
                Description   = description,
                Amount        = amount,
                IsValidate    = true
            };
            await query.Start(client);

            query.IsValidate = false;
            await query.Start(client);
            return query.ReceiptNo;
        }
    }
}