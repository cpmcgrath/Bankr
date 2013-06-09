using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Phone.Controls;
using CMcG.Bankr.ViewModels.Options;

namespace CMcG.Bankr.Views.Options
{
    public partial class ScreenProtectionView : PhoneApplicationPage
    {
        public ScreenProtectionView()
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
            var vm = (ScreenProtectionViewModel)DataContext;
            vm.Save();
            this.Navigation().GoBack();
        }
    }
}