using System;
using System.Linq;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using CMcG.Bankr.ViewModels;
using CMcG.Bankr.Data;
using System.Windows;

namespace CMcG.Bankr.Views
{
    public partial class TransactionsView : PhoneApplicationPage
    {
        public TransactionsView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.SetupView(e);
        }

        void ShowSettings(object sender, EventArgs e)
        {
            this.Navigation().GoTo<ViewModels.Options.OptionsViewModel>();
        }

        void ShowReplacements(object sender, EventArgs e)
        {
            this.Navigation().GoTo<ViewModels.Options.ReplacementsViewModel>();
        }

        void ShowUpcoming(object sender, EventArgs e)
        {
            this.Navigation().GoTo<UpcomingTransactionsViewModel>();
        }

        void TransferMoney(object sender, EventArgs e)
        {
            this.Navigation().GoTo<ViewModels.Transfer.PickAccountViewModel>();
        }

        void MarkAllAsSeen(object sender, EventArgs e)
        {
            ((TransactionsViewModel)DataContext).MarkAsSeen();
        }

        void RefreshAccount(object sender, EventArgs e)
        {
            ((TransactionsViewModel)DataContext).Refresh();
        }

        void AddReplacement(object sender, EventArgs e)
        {
            var menuItem    = (MenuItem)sender;
            var menu        = (ContextMenu)menuItem.Parent;
            var ctl         = (FrameworkElement)menu.Owner;
            var transaction = (Transaction)ctl.DataContext;

            this.Navigation().GoTo<ViewModels.Options.ReplacementEditViewModel>(-1, transaction.Id);
        }

        void GoToWebsite(object sender, EventArgs e)
        {
            var url = @"http://www.netbank.com.au/mobile";
            new Microsoft.Phone.Tasks.WebBrowserTask { Uri = new Uri(url) }.Show();
        }
    }
}