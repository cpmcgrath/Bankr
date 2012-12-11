using System;
using System.Linq;
using CMcG.CommonwealthBank.Data;
using CMcG.CommonwealthBank.Logic;
using Microsoft.Phone.Shell;

namespace CMcG.CommonwealthBank.Agent
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
            var retriever = new DataRetriever { Status = new AppStatus() };
            retriever.Callback = () =>
            {
                UpdateLiveTile();
                afterUpdate.Invoke();
            };
            return retriever.LoadData();
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
                var transactions = account.TransactionList.Where(x => !x.IsSeen).Select(x => x.Summary).Take(2).ToArray();
                var backContent = count == 1 ? "New unseen transaction"
                                : count  > 1 ? count + " unseen transactions"
                                : "";
                var backTitle   = count > 0 ? amount : "";

                tile.Update(new IconicTileData
                {
                    Title = amount,
                    Count = count,
                    WideContent1 = backContent,
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
