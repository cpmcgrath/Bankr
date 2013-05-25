using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using CMcG.CommonwealthBank.ViewModels.Transfer;

namespace CMcG.CommonwealthBank.Views.Transfer
{
    public partial class FinishTransferView : PhoneApplicationPage
    {
        public FinishTransferView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.SetupView(e);
        }

        void MakeTransaction(object sender, EventArgs e)
        {
            this.FinishBinding();
            var vm = (FinishTransferViewModel)DataContext;
            string message = string.Format("Are you sure you want complete the transfer?\r\nFrom: {0}\r\nTo: {1}\r\n{2:c}\r\n{3}",
                                           vm.FromAccount.AccountName, vm.ToAccount.AccountName, vm.Amount, vm.Description);
            var result = MessageBox.Show(message, "Confirmation", MessageBoxButton.OKCancel);
            if (result != MessageBoxResult.OK)
                return;

            vm.MakeTransaction(this.Navigation());
        }

        void Cancel(object sender, EventArgs e)
        {
            this.Navigation().GoBack(3);
        }

    }
}