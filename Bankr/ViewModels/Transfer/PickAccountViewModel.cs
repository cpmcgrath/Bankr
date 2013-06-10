using System;
using System.Linq;
using System.Collections.Generic;
using CMcG.Bankr.Data;
using System.ComponentModel;
using Caliburn.Micro;

namespace CMcG.Bankr.ViewModels.Transfer
{
    [Description("1. From Account")]
    public class PickAccountViewModel : ViewModelBase
    {
        public PickAccountViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        protected override void OnLoad()
        {
            using (var store = new DataStoreContext())
            {
                YourAccounts = store.Accounts.ToArray();
            }

            FirePropertyChanged(() => YourAccounts);
        }

        public Account[] YourAccounts { get; set; }

        public void SelectAccount(Account account)
        {
            Navigator.UriFor<PickRecipientViewModel>()
                     .WithParam(p => p.FromAccountId, account.Id)
                     .Navigate();
        }
    }
}
