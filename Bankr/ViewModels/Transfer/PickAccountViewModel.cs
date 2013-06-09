using System;
using System.Linq;
using System.Collections.Generic;
using CMcG.CommonwealthBank.Data;
using System.ComponentModel;

namespace CMcG.CommonwealthBank.ViewModels.Transfer
{
    [Description("1. From Account")]
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
