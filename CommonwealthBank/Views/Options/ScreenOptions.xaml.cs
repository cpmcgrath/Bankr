using System;
using System.Linq;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using CMcG.CommonwealthBank.ViewModels;

namespace CMcG.CommonwealthBank.Views.Options
{
    public partial class ScreenOptions : PhoneApplicationPage
    {
        public ScreenOptions()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.CheckPermissions<OptionsViewModel>();
        }

        void Save(object sender, EventArgs e)
        {
            this.FinishBinding();
            ((OptionsViewModel)DataContext).Save();
            NavigationService.GoBack();
        }

        private void SendErrorReport(object sender, System.Windows.RoutedEventArgs e)
        {
            ((OptionsViewModel)DataContext).SendErrorReport();
        }
    }
}