using System;
using System.Linq;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using CMcG.CommonwealthBank.ViewModels;

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

            var argLookup = NavigationContext.QueryString;
            if (argLookup.ContainsKey("transid"))
            {
                int transId = int.Parse(argLookup["transid"]);
                this.CheckPermissions(() => ReplacementEditViewModel.Create(transId));
            }
            else
            {
                int id = int.Parse(argLookup["id"]);
                this.CheckPermissions(() => ReplacementEditViewModel.Load(id));
            }
        }

        void Save(object sender, EventArgs e)
        {
            this.FinishBinding();
            ((ReplacementEditViewModel)DataContext).Save();
            NavigationService.GoBack();
        }

        void Delete(object sender, EventArgs e)
        {
            this.FinishBinding();
            ((ReplacementEditViewModel)DataContext).Delete();
            NavigationService.GoBack();
        }
    }
}