using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using CMcG.CommonwealthBank.ViewModels.Transfer;
using Microsoft.Phone.Shell;

namespace CMcG.CommonwealthBank.Views.Transfer
{
    public partial class AmountView : PhoneApplicationPage
    {
        public AmountView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.SetupView(e);
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
            Confirmation(decimal.Parse(amount.Substring(1)));
        }

        void GoToConfirmation(object sender, EventArgs e)
        {
            Confirmation(ViewModel.Value);
        }

        void Confirmation(decimal amount)
        {
            this.Navigation().GoTo<FinishTransferViewModel>(ViewModel.FromAccountId, ViewModel.ToAccountId, amount);
        }

        void Cancel(object sender, EventArgs e)
        {
            this.Navigation().GoBack(3);
        }

        
    }
}