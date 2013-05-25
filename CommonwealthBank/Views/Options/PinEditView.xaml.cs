using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Phone.Controls;
using CMcG.CommonwealthBank.ViewModels.Options;

namespace CMcG.CommonwealthBank.Views.Options
{
    public partial class PinEditView : PhoneApplicationPage
    {
        public PinEditView()
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
            ((PinEditViewModel)DataContext).Save();
            this.Navigation().GoBack();
        }
    }
}