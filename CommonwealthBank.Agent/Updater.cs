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
                var account = store.CurrentOptions.GetSelectedAccount(store);

                if (account == null)
                    return false;

                var sinceUpdate = DateTime.Now - account.LastUpdate;
                if (sinceUpdate > TimeSpan.FromMinutes(45))
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
                var backContent = count == 1 ? account.TransactionList.First(x => !x.IsSeen).Summary
                                : count  > 0 ? count + " unseen transactions"
                                : "";
                var backTitle   = count > 0 ? amount : "";

                tile.Update(new StandardTileData { Title = amount, BackTitle = backTitle, BackContent = backContent, Count = count });
            }
        }
    }
}
