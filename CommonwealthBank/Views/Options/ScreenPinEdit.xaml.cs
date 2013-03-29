using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Phone.Controls;
using CMcG.CommonwealthBank.ViewModels.Options;

namespace CMcG.CommonwealthBank.Views.Options
{
    public partial class ScreenPinEdit : PhoneApplicationPage
    {
        public ScreenPinEdit()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.CheckPermissions<PinEditViewModel>();
        }

        void Save(object sender, EventArgs e)
        {
            this.FinishBinding();
            ((PinEditViewModel)DataContext).Save();
            NavigationService.GoBack();
        }
    }
}