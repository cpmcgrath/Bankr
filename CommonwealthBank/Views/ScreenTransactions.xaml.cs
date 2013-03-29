using System;
using System.Linq;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using CMcG.CommonwealthBank.ViewModels;
using CMcG.CommonwealthBank.Data;
using System.Windows;

namespace CMcG.CommonwealthBank.Views
{
    public partial class ScreenTransactions : PhoneApplicationPage
    {
        public ScreenTransactions()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.CheckPermissions<TransactionViewModel>();
        }

        void ShowSettings(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Options/ScreenOptions.xaml", UriKind.Relative));
        }

        void ShowReplacements(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Options/ScreenReplacements.xaml", UriKind.Relative));
        }

        void ShowUpcoming(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/ScreenUpcomingTransactions.xaml", UriKind.Relative));
        }

        void MarkAllAsSeen(object sender, EventArgs e)
        {
            ((TransactionViewModel)DataContext).MarkAsSeen();
        }

        void RefreshAccount(object sender, EventArgs e)
        {
            ((TransactionViewModel)DataContext).Refresh();
        }

        void AddReplacement(object sender, EventArgs e)
        {
            var menuItem    = (MenuItem)sender;
            var menu        = (ContextMenu)menuItem.Parent;
            var ctl         = (FrameworkElement)menu.Owner;
            var transaction = (Transaction)ctl.DataContext;

            var url = "/Views/Options/ScreenReplacementEdit.xaml?transid=" + transaction.Id;
            NavigationService.Navigate(new Uri(url, UriKind.Relative));
        }

        void GoToWebsite(object sender, EventArgs e)
        {
            var url = @"http://www.netbank.com.au/mobile";
            new Microsoft.Phone.Tasks.WebBrowserTask { Uri = new Uri(url) }.Show();
        }
    }
}