using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using CMcG.CommonwealthBank.ViewModels.Transfer;
using CMcG.CommonwealthBank.Data;

namespace CMcG.CommonwealthBank.Views.Transfer
{
    public partial class ScreenPickRecipient : PhoneApplicationPage
    {
        public ScreenPickRecipient()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var argLookup = NavigationContext.QueryString;
            int fromId = int.Parse(argLookup["fromAccountId"]);
            this.CheckPermissions(e, () => new PickRecipientViewModel(fromId));
        }

        void SelectRecipient(object sender, RoutedEventArgs e)
        {
            var vm = (PickRecipientViewModel)DataContext;

            var ctl     = (FrameworkElement)sender;
            var account = (TransferToAccount)ctl.DataContext;

            this.Navigation().GoTo<FinishTransferViewModel>(vm.FromAccount.Id, account.Id);
        }

        public void Refresh(object sender, EventArgs e)
        {
            var vm = (PickRecipientViewModel)DataContext;
            vm.ReloadList();
        }
    }
}