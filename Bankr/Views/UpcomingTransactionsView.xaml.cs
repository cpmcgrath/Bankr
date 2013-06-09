using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Phone.Controls;
using CMcG.CommonwealthBank.ViewModels;

namespace CMcG.CommonwealthBank.Views
{
    public partial class UpcomingTransactionsView : PhoneApplicationPage
    {
        public UpcomingTransactionsView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.SetupView(e);
        }
    }
}