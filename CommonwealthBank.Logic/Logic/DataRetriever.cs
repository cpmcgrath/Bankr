using System;
using System.Linq;
using CMcG.CommonwealthBank.Data;
using System.Net.Http;

namespace CMcG.CommonwealthBank.Logic
{
    public class DataRetriever
    {
        public AppStatus Status   { get; set; }
        public Action    Callback { get; set; }

        string m_sessionId;

        public LoginDetails GetLogonDetails()
        {
            using (var store = new DataStoreContext())
                return store.LoginDetails.FirstOrDefault();
        }

        public bool CanLoadData
        {
            get { return GetLogonDetails() != null; }
        }

        public async void LoadData()
        {
            var client     = new HttpClient { BaseAddress = new Uri("https://www2.my.commbank.com.au") };
            var logonQuery = GetLogonQuery();

            m_sessionId = await logonQuery.Start(client);

            if (m_sessionId != null)
            {
                await SetupQuery(new GetTransactionsQuery()        ).Start(client);
                await SetupQuery(new GetUpcomingTransactionsQuery()).Start(client);
            }
            Callback();
        }

        CommBankQuery SetupQuery(CommBankQuery query)
        {
            query.Status    = Status;
            query.SessionId = m_sessionId;
            return query;
        }

        LogonQuery GetLogonQuery()
        {
            var loginDetails = GetLogonDetails();
            var logonQuery = new LogonQuery
            {
                Username = loginDetails.Username,
                Password = new TwoWayEncryption().Decrypt(loginDetails.Password),
            };
            SetupQuery(logonQuery);
            return logonQuery;
        }
    }
}