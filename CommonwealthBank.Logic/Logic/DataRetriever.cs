using System;
using System.Linq;
using System.Net.Http;
using CMcG.CommonwealthBank.Data;
using System.Threading.Tasks;
using CMcG.CommonwealthBank.Logic.Queries;

namespace CMcG.CommonwealthBank.Logic
{
    public class DataRetriever
    {
        public AppStatus Status   { get; set; }
        public Action    Callback { get; set; }

        string m_sessionId;

        public bool CanLoadData
        {
            get { return LogonQuery.GetLogonDetails() != null; }
        }

        public async void LoadData()
        {
            var client = new HttpClient { BaseAddress = new Uri("https://www2.my.commbank.com.au") };

            m_sessionId = await RunQuery<LogonQuery>(client);
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
            var client = new HttpClient { BaseAddress = new Uri("https://www2.my.commbank.com.au") };

            m_sessionId = await RunQuery<LogonQuery>(client);
            if (m_sessionId != null)
            {
                await RunQuery<GetTransferAccountsQuery>(client);
            }
            Callback();
        }

        async Task<string> RunQuery<T>(HttpClient client) where T : CommBankQuery, new()
        {
            return await new T
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
            var client = new HttpClient { BaseAddress = new Uri("https://www2.my.commbank.com.au") };

            m_sessionId = await RunQuery<LogonQuery>(client);
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