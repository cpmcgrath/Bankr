using System;
using System.Linq;
using CMcG.CommonwealthBank.Data;

namespace CMcG.CommonwealthBank.Logic
{
    public class DataRetriever
    {
        public AppStatus Status   { get; set; }
        public Action    Callback { get; set; }

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

            var client = new CookieAwareWebClient { BaseAddress = "https://www2.my.commbank.com.au" };

            new LogonQuery
            {
                Username = loginDetails.Username,
                Password = new TwoWayEncryption().Decrypt(loginDetails.Password),
                Status   = Status,
                Callback = Callback,
                Next     = new GetTransactionsQuery()
            }.Start(client);

            return true;
        }
    }
}