using System;
using System.Linq;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using CMcG.Bankr.Data;
using System.Windows;
using CMcG.Bankr.ViewModels;

namespace CMcG.Bankr.Views
{
    public partial class TransactionsView : PhoneApplicationPage
    {
        public TransactionsView()
        {
            InitializeComponent();
        }

        void AddReplacement(object sender, EventArgs e)
        {
            var menuItem    = (MenuItem)sender;
            var menu        = (ContextMenu)menuItem.Parent;
            var ctl         = (FrameworkElement)menu.Owner;
            var transaction = (Transaction)ctl.DataContext;

            var vm = (TransactionsViewModel)DataContext;
            vm.CreateReplacement(transaction.Id);
        }
    }
}