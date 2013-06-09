using System;
using System.Linq;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using CMcG.Bankr.ViewModels.Options;

namespace CMcG.Bankr.Views.Options
{
    public partial class OptionsView : PhoneApplicationPage
    {
        public OptionsView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.SetupView(e);
        }

        void Save(object sender, EventArgs e)
        {
            this.FinishBinding();
            ((OptionsViewModel)DataContext).Save();
            this.Navigation().GoBack();
        }

        private void SendErrorReport(object sender, System.Windows.RoutedEventArgs e)
        {
            ((OptionsViewModel)DataContext).SendErrorReport();
        }

        void ShowPinOptions(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Navigation().GoTo<PinEditViewModel>();
        }

        void ShowScreenProtection(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Navigation().GoTo<ScreenProtectionViewModel>();
        }
    }
}