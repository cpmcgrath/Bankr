using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using CMcG.Bankr.ViewModels.Transfer;
using Microsoft.Phone.Shell;

namespace CMcG.Bankr.Views.Transfer
{
    public partial class AmountView : PhoneApplicationPage
    {
        public AmountView()
        {
            InitializeComponent();
        }

        AmountViewModel ViewModel
        {
            get { return (AmountViewModel)DataContext; }
        }

        void OnNumberPressed(object sender, RoutedEventArgs e)
        {
            ViewModel.Amount += (string)((Button)sender).Content;
        }

        void Clear(object sender, RoutedEventArgs e)
        {
            ViewModel.Amount = "$";
        }

        void ChooseAmount(object sender, EventArgs e)
        {
            string amount;
            if (sender is Button)
                amount = (string)((Button)sender).Content;
            else
            {
                var text = ((ApplicationBarMenuItem)sender).Text;
                amount = text.Substring(text.IndexOf('$'));
            }

            ViewModel.SelectAmount(decimal.Parse(amount.Substring(1)));
        }
    }
}