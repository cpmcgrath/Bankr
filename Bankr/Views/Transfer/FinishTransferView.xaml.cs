using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using CMcG.Bankr.ViewModels.Transfer;

namespace CMcG.Bankr.Views.Transfer
{
    public partial class FinishTransferView : PhoneApplicationPage
    {
        public FinishTransferView()
        {
            InitializeComponent();
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

            vm.MakeTransaction();
        }
    }
}