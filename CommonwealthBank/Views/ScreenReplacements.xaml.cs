using System;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using CMcG.CommonwealthBank.Data;
using CMcG.CommonwealthBank.ViewModels;

namespace CMcG.CommonwealthBank.Views
{
    public partial class ScreenReplacements : PhoneApplicationPage
    {
        public ScreenReplacements()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.CheckPermissions<ReplacementsViewModel>();
        }

        void ViewReplacement(object sender, EventArgs e)
        {
            var ctl         = (FrameworkElement)sender;
            var replacement = (Replacement)ctl.DataContext;

            var url = "/Views/ScreenReplacementEdit.xaml?id=" + replacement.Id;
            NavigationService.Navigate(new Uri(url, UriKind.Relative));
        }
    }
}