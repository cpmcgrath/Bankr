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
        public PickRecipientViewModel(int fromAccountId)
        {
            using (var store = new DataStoreContext())
            {
                FromAccount = store.Accounts.First(x => x.Id == fromAccountId);
            }
        }

        public Account FromAccount { get; private set; }

        public TransferToAccount[] ToAccounts
        {
            get
            {
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
    }
}