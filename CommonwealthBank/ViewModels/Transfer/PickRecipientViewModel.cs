using CMcG.CommonwealthBank.Data;
using CMcG.CommonwealthBank.Logic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CMcG.CommonwealthBank.ViewModels.Transfer
{
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