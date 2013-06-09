using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using CMcG.Bankr.ViewModels.Transfer;
using CMcG.Bankr.Data;

namespace CMcG.Bankr.Views.Transfer
{
    public partial class PickAccountView : PhoneApplicationPage
    {
        public PickAccountView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.SetupView(e);
        }

        void SelectAccount(object sender, RoutedEventArgs e)
        {
            var ctl     = (FrameworkElement)sender;
            var account = (Account)ctl.DataContext;

            this.Navigation().GoTo<PickRecipientViewModel>(account.Id);
        }
    }
}