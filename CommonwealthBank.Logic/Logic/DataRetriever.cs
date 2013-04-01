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

        async Task<string> RunQuery<T>(HttpClient client) where T : CommBankQuery, new()
        {
            return await new T
            {
                Status    = Status,
                SessionId = m_sessionId
            }.Start(client);
        }
    }
}