using System;
using System.Linq;
using System.Data.Linq;
using System.Collections.Generic;
using Microsoft.Phone.Data.Linq;

namespace CMcG.Bankr.Data
{
    public partial class DataStoreUpdater
    {
        const int CURRENT_VERSION = 6;

        void Create(DataStoreContext store)
        {
            store.CreateDatabase();
            var schemaUpdater = store.CreateDatabaseSchemaUpdater();
            schemaUpdater.DatabaseSchemaVersion = CURRENT_VERSION;
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
                case 0 : schema.AddColumn<Options>("AutoRefresh");        goto case 1;
                case 1 : schema.AddTable<UpcomingTransaction>();          goto case 2;
                case 2 : schema.AddColumn<LoginDetails>("Pin");           goto case 3;
                case 3 : schema.AddTable<TransferToAccount>();            goto case 4;
                case 4 : schema.AddColumn<TransferToAccount>("SenderId"); goto case 5;
                case 5 : schema.AddTable<ScreenSecurity>();               break;

                default : return;
            }

            schema.DatabaseSchemaVersion = CURRENT_VERSION;
            schema.Execute();
        }
    }
}