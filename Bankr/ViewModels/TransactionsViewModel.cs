using System;
using System.Linq;
using CMcG.Bankr.Agent;
using CMcG.Bankr.Data;
using CMcG.Bankr.Logic;
using System.Text.RegularExpressions;
using Caliburn.Micro;

namespace CMcG.Bankr.ViewModels
{
    public class TransactionsViewModel : ViewModelBase
    {
        public TransactionsViewModel(INavigationService navigationService) : base(navigationService)
        {
            Load();
        }

        protected override void OnLoad()
        {
            if (Account != null)
            {
                var sinceUpdate = DateTime.Now - Account.LastUpdate;
                CurrentApp.Status.SetAction(sinceUpdate.ToFormattedString() + " since last update", true);
                if (sinceUpdate > TimeSpan.FromMinutes(45) && AutoRefresh)
                    RefreshData();
            }
            else
                RefreshData();
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
            Refresh();
            new Updater().UpdateLiveTile();
        }

        public void RefreshData()
        {
            new DataRetriever
            {
                Status   = CurrentApp.Status,
                Callback = Load
            }.LoadData();
        }

        public bool          AutoRefresh  { get; set; }
        public Account       Account      { get; set; }

        Transaction[] m_allTransactions = new Transaction[0];

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

        public void GoToWebsite()
        {
            var url = @"http://www.netbank.com.au/mobile";
            new Microsoft.Phone.Tasks.WebBrowserTask { Uri = new Uri(url) }.Show();
        }

        public void GoToMoneyTransfer()
        {
            Navigator.UriFor<Transfer.PickAccountViewModel>().Navigate();
        }

        public void GoToSettings()
        {
            Navigator.UriFor<Options.OptionsViewModel>().Navigate();
        }

        public void GoToReplacements()
        {
            Navigator.UriFor<Options.ReplacementsViewModel>().Navigate();
        }

        public void GoToUpcoming()
        {
            Navigator.UriFor<UpcomingTransactionsViewModel>().Navigate();
        }

        public void CreateReplacement(int transactionId)
        {
            Navigator.UriFor<Options.ReplacementEditViewModel>()
                    .WithParam(x => x.TransactionId, transactionId)
                    .Navigate();
        }
    }
}
