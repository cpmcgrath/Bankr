using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using CMcG.CommonwealthBank.ViewModels.Transfer;
using CMcG.CommonwealthBank.Data;

namespace CMcG.CommonwealthBank.Views.Transfer
{
    public partial class ScreenPickAccount : PhoneApplicationPage
    {
        public ScreenPickAccount()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.CheckPermissions<PickAccountViewModel>(e);
        }

        void SelectAccount(object sender, RoutedEventArgs e)
        {
            var ctl     = (FrameworkElement)sender;
            var account = (Account)ctl.DataContext;

            var url = "/Views/Transfer/ScreenPickRecipient.xaml?fromid=" + account.Id;
            NavigationService.Navigate(new Uri(url, UriKind.Relative));
        }
    }
}