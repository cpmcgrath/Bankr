using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using CMcG.Bankr.ViewModels.Transfer;
using CMcG.Bankr.Data;

namespace CMcG.Bankr.Views.Transfer
{
    public partial class PickRecipientView : PhoneApplicationPage
    {
        public PickRecipientView()
        {
            InitializeComponent();
        }

        void SelectRecipient(object sender, RoutedEventArgs e)
        {
            var vm = (PickRecipientViewModel)DataContext;

            var ctl     = (FrameworkElement)sender;
            var account = (TransferToAccount)ctl.DataContext;

            vm.SelectRecipient(account);
        }
    }
}