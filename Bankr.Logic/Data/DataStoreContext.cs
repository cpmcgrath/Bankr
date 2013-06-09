using System;
using System.Linq;
using System.Data.Linq;
using System.Collections.Generic;

namespace CMcG.CommonwealthBank.Data
{
    public partial class DataStoreContext : System.Data.Linq.DataContext
    {
        public DataStoreContext(string connectionString = "Data Source=isostore:/DataStore.sdf") : base(connectionString)
        {
            new DataStoreUpdater().Upgrade(this);
        }

        public Table<Account> Accounts
        {
            get { return GetTable<Account>(); }
        }

        public Table<Error> Errors
        {
            get { return GetTable<Error>(); }
        }

        public Table<LoginDetails> LoginDetails
        {
            get { return GetTable<LoginDetails>(); }
        }

        public Table<Options> Options
        {
            get { return this.GetTable<Options>(); }
        }

        public Options CurrentOptions
        {
            get { return Options.FirstOrDefault() ?? new Options(); }
        }

        public Table<Transaction> Transactions
        {
            get { return GetTable<Transaction>(); }
        }

        public Table<UpcomingTransaction> UpcomingTransactions
        {
            get { return GetTable<UpcomingTransaction>(); }
        }

        public Table<Replacement> Replacements
        {
            get { return GetTable<Replacement>(); }
        }

        public Table<TransferToAccount> TransferToAccounts
        {
            get { return GetTable<TransferToAccount>(); }
        }

        public Table<ScreenSecurity> ScreenSecurity
        {
            get { return GetTable<ScreenSecurity>(); }
        }
    }
}