using System;
using System.Linq;
using CMcG.Bankr.Agent;
using CMcG.Bankr.Data;
using CMcG.Bankr.Logic;
using System.Text.RegularExpressions;

namespace CMcG.Bankr.ViewModels
{
    public class TransactionsViewModel : ViewModelBase
    {
        public TransactionsViewModel()
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

                m_allTransactions = store.Transactions.OrderByDescending(x => x.Id).ToArray();
                var replacements  = store.Replacements.ToArray();

                foreach (var transaction in m_allTransactions)
                    transaction.Replacements = replacements;
            }
            NotifyPropertyChanged("Transactions", "ReceivedTransactions", "SentTransactions", "InternalTransactions", "AccountAmount");
            new Updater().UpdateLiveTile();
        }

        public void Refresh()
        {
            new DataRetriever
            {
                Status   = CurrentApp.Status,
                Callback = Load
            }.LoadData();
        }

        public bool          AutoRefresh  { get; set; }
        public Account       Account      { get; set; }

        Transaction[] m_allTransactions;

        public Transaction[] Transactions
        {
            get { return m_allTransactions.Except(InternalTransactions).ToArray(); }
        }

        public Transaction[] ReceivedTransactions
        {
            get { return Transactions.Where(x => x.Amount > 0).ToArray(); }
        }

        public Transaction[] SentTransactions
        {
            get { return Transactions.Where(x => x.Amount < 0).ToArray(); }
        }

        public Transaction[] InternalTransactions
        {
            get { return m_allTransactions.Where(IsInternal).ToArray(); }
        }

        bool IsInternal(Transaction trans)
        {
            var match = Regex.Match(trans.Description, @"Transfer (from|to) xx(?<digits>\d+) NetBank");
            return match.Success;
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
