using System;
using System.Linq;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using CMcG.CommonwealthBank.ViewModels.Options;

namespace CMcG.CommonwealthBank.Views.Options
{
    public partial class ScreenReplacementEdit : PhoneApplicationPage
    {
        public ScreenReplacementEdit()
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
            ((ReplacementEditViewModel)DataContext).Save();
            this.Navigation().GoBack();
        }

        void Delete(object sender, EventArgs e)
        {
            this.FinishBinding();
            ((ReplacementEditViewModel)DataContext).Delete();
            this.Navigation().GoBack();
        }
    }
}