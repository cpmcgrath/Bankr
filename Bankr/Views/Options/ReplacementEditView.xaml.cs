using System;
using System.Linq;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using CMcG.Bankr.ViewModels.Options;

namespace CMcG.Bankr.Views.Options
{
    public partial class ReplacementEditView : PhoneApplicationPage
    {
        public ReplacementEditView()
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