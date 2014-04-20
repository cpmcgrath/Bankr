using System;
using System.Linq;
using CMcG.Bankr.Data;
using CMcG.Bankr.Logic;
using Microsoft.Phone.Shell;

namespace CMcG.Bankr.Agent
{
    public class Updater
    {
        public bool Update(Action afterUpdate)
        {
            using (var store = new DataStoreContext())
            {
                bool autoRefresh = store.CurrentOptions.AutoRefresh;
                var  account     = store.CurrentOptions.GetSelectedAccount(store);
                bool hasLogon    = store.LoginDetails.Any();

                if (account == null || !hasLogon)
                {
                    SetLiveTileToError();
                    return false;
                }

                var sinceUpdate = DateTime.Now - account.LastUpdate;
                if (sinceUpdate > TimeSpan.FromMinutes(45) && autoRefresh)
                    return Refresh(store, afterUpdate);
            }

            return false;
        }

        public bool Refresh(DataStoreContext store, Action afterUpdate)
        {
            var retriever = new DataRetriever { Status = new AppStatus { UseToast = true } };
            retriever.Callback = () =>
            {
                UpdateLiveTile();
                afterUpdate.Invoke();
            };
            
            bool canLoad = retriever.CanLoadData;
            
            if (canLoad)
                retriever.LoadData();

            return canLoad;
        }

        public void UpdateLiveTile()
        {
            using (var store = new DataStoreContext())
            {
                var account = store.CurrentOptions.GetSelectedAccount(store);

                if (account == null)
                    return;

                string amount = account.AccountAmount.ToString("$0.00");

                var tile        = ShellTile.ActiveTiles.First();
                var count       = account.TransactionList.Count(x => !x.IsSeen);
                var transactions = store.UpcomingTransactions.OrderBy(x => x.EffectiveDate).Select(x => x.Summary).ToArray();

                tile.Update(new IconicTileData
                {
                    Title = amount,
                    Count = count,
                    WideContent1 = "Upcoming Transactions",
                    WideContent2 = transactions.FirstOrDefault(),
                    WideContent3 = transactions.Skip(1).FirstOrDefault()
                });
            }
        }

        public void SetLiveTileToError()
        {
            var tile = ShellTile.ActiveTiles.First();
            tile.Update(new IconicTileData { Title = "Requires Attention", Count = 0 });
        }
    }
}
