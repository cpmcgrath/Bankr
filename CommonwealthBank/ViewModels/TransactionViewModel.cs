using System;
using System.Linq;
using CMcG.CommonwealthBank.Agent;
using CMcG.CommonwealthBank.Data;
using CMcG.CommonwealthBank.Logic;

namespace CMcG.CommonwealthBank.ViewModels
{
    public class TransactionViewModel : ViewModelBase
    {
        public TransactionViewModel()
        {
            Load();

            if (Account != null)
            {
                var sinceUpdate = DateTime.Now - Account.LastUpdate;
                CurrentApp.Status.SetAction(sinceUpdate.ToFormattedString() + " since last update", true);
                if (sinceUpdate > TimeSpan.FromMinutes(45) && AutoRefresh)
                    Refresh();
            }
            else
                Refresh();
        }

        void Load()
        {
            using (var store = new DataStoreContext())
            {
                Account          = store.CurrentOptions.GetSelectedAccount(store);
                AutoRefresh      = store.CurrentOptions.AutoRefresh;

                Transactions     = store.Transactions.OrderByDescending(x => x.Id).ToArray();
                var replacements = store.Replacements.ToArray();

                foreach (var transaction in Transactions)
                    transaction.Replacements = replacements;
            }
            NotifyPropertyChanged("Transactions", "ReceivedTransactions", "SentTransactions", "AccountAmount");
            new Updater().UpdateLiveTile();
        }

        public void Refresh()
        {
            var retriever = new DataRetriever
            {
                Status   = CurrentApp.Status,
                Callback = Load
            }.LoadData();
        }

        public bool          AutoRefresh  { get; set; }
        public Account       Account      { get; set; }
        public Transaction[] Transactions { get; set; }

        public Transaction[] ReceivedTransactions
        {
            get { return Transactions.Where(x => x.Amount > 0).ToArray(); }
        }

        public Transaction[] SentTransactions
        {
            get { return Transactions.Where(x => x.Amount < 0).ToArray(); }
        }

        public string AccountAmount
        {
            get { return Account != null ? Account.AccountAmount.ToString("$0.00") : ""; }
        }

        public void MarkAsSeen()
        {
            using (var store = new DataStoreContext())
            {
                foreach (var transaction in store.Transactions)
                    transaction.IsSeen = true;

                store.SubmitChanges();
            }
            Load();
        }
    }
}
