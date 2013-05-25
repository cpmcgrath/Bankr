using System;
using System.Linq;
using System.Collections.Generic;
using CMcG.CommonwealthBank.Data;

namespace CMcG.CommonwealthBank.ViewModels.Transfer
{
    public class PickAccountViewModel : ViewModelBase
    {
        public PickAccountViewModel()
        {
            Load();
        }

        void Load()
        {
            using (var store = new DataStoreContext())
            {
                YourAccounts = store.Accounts.ToArray();
            }

            NotifyPropertyChanged("YourAccounts");
        }

        public Account[] YourAccounts { get; set; }
    }
}
