using Caliburn.Micro;
using CMcG.Bankr.Data;
using CMcG.Bankr.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CMcG.Bankr.ViewModels.Transfer
{
    [Description("2. To Account")]
    public class PickRecipientViewModel : ViewModelBase
    {
        public PickRecipientViewModel(INavigationService navigationService) : base(navigationService) { }

        public int FromAccountId
        {
            get { return FromAccount.Id; }
            private set
            {
                using (var store = new DataStoreContext())
                    FromAccount = store.Accounts.First(x => x.Id == value);
            }
        }

        public Account FromAccount { get; private set; }

        public TransferToAccount[] ToAccounts
        {
            get
            {
                if (FromAccount == null)
                    return null;

                using (var store = new DataStoreContext())
                {
                    if (!store.TransferToAccounts.Any())
                        ReloadList();

                    bool linkedOnly = FromAccount.AccountName == "NetBank Saver";

                    return store.TransferToAccounts
                                .Where(x => !linkedOnly || x.Type == TransferToAccount.AccountType.Linked)
                                .Where(x => x.Type != TransferToAccount.AccountType.Linked || x.AccountName != FromAccount.AccountName)
                                .ToArray();
                }
            }
        }

        public void ReloadList()
        {
            new DataRetriever
            {
                Status   = CurrentApp.Status,
                Callback = () => NotifyPropertyChanged("ToAccounts")
            }.LoadTransferAccounts();
        }

        public void SelectRecipient(TransferToAccount account)
        {
            Navigator.UriFor<AmountViewModel>()
                     .WithParam(p => p.FromAccountId, FromAccountId)
                     .WithParam(p => p.ToAccountId,   account.Id)
                     .Navigate();
        }
    }
}