using System;
using System.Linq;
using System.Data.Linq;
using System.Collections.Generic;
using Microsoft.Phone.Data.Linq;

namespace CMcG.CommonwealthBank.Data
{
    public partial class DataStoreContext : System.Data.Linq.DataContext
    {
        public void CreateIfNotExists()
        {
            if (!DatabaseExists())
            {
                CreateDatabase();
                var schemaUpdater = this.CreateDatabaseSchemaUpdater();
                schemaUpdater.DatabaseSchemaVersion = 1;
                schemaUpdater.Execute();
            }
            else
            {
                var schemaUpdater = this.CreateDatabaseSchemaUpdater();
                int version       = schemaUpdater.DatabaseSchemaVersion;
 
                if (version == 0)
                {
                    schemaUpdater.AddColumn<Options>("AutoRefresh");
                    schemaUpdater.DatabaseSchemaVersion = 1;
                    schemaUpdater.Execute();
                }
            }
        }

        public DataStoreContext(string connectionString = "Data Source=isostore:/DataStore.sdf") : base(connectionString)
        {
            CreateIfNotExists();
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

        public Table<Replacement> Replacements
        {
            get { return GetTable<Replacement>(); }
        }
    }
}