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

            var argLookup = NavigationContext.QueryString;
            if (argLookup.ContainsKey("transactionId"))
            {
                int transId = int.Parse(argLookup["transactionId"]);
                this.CheckPermissions(e, () => new ReplacementEditViewModel(-1, transId));
            }
            else
            {
                int id = int.Parse(argLookup["id"]);
                this.CheckPermissions(e, () => new ReplacementEditViewModel(id));
            }
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