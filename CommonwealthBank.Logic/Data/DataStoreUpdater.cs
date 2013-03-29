using System;
using System.Linq;
using System.Data.Linq;
using System.Collections.Generic;
using Microsoft.Phone.Data.Linq;

namespace CMcG.CommonwealthBank.Data
{
    public partial class DataStoreUpdater
    {
        void Create(DataStoreContext store)
        {
            store.CreateDatabase();
            var schemaUpdater = store.CreateDatabaseSchemaUpdater();
            schemaUpdater.DatabaseSchemaVersion = 3;
            schemaUpdater.Execute();
        }

        public void Upgrade(DataStoreContext store)
        {
            if (!store.DatabaseExists())
            {
                Create(store);
                return;
            }

            var schema  = store.CreateDatabaseSchemaUpdater();
            int version = schema.DatabaseSchemaVersion;

            switch (version)
            {
                case 0 : schema.AddColumn<Options>("AutoRefresh"); goto case 1;
                case 1 : schema.AddTable<UpcomingTransaction>();   goto case 2;
                case 2 : schema.AddColumn<LoginDetails>("Pin");    break;
                case 3 : return;
            }

            schema.DatabaseSchemaVersion = 3;
            schema.Execute();
        }
    }
}