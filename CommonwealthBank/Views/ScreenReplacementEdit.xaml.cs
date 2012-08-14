using System;
using System.Linq;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using CMcG.CommonwealthBank.ViewModels;

namespace CMcG.CommonwealthBank.Views
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

            var argLookup = NavigationContext.QueryString;
            var transId   = int.Parse(argLookup["id"]);
            this.CheckPermissions(() => new ReplacementEditViewModel(transId));
        }

        void Save(object sender, EventArgs e)
        {
            this.FinishBinding();
            ((ReplacementEditViewModel)DataContext).Save();
            NavigationService.GoBack();
        }
    }
}