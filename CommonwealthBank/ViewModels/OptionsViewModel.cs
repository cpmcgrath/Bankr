﻿using System;
using System.Linq;
using CMcG.CommonwealthBank.Data;
using Microsoft.Phone.Tasks;

namespace CMcG.CommonwealthBank.ViewModels
{
    public class OptionsViewModel : ViewModelBase
    {
        public OptionsViewModel()
        {
            using (var store = new DataStoreContext())
            {
                Accounts = store.Accounts.ToArray();

                if (store.Options.Any())
                    SelectedAccount = store.CurrentOptions.GetSelectedAccount(store);
            }
        }

        public Account[] Accounts { get; set; }

        private Account m_selectedAccount;
        public Account SelectedAccount
        {
            get { return m_selectedAccount; }
            set
            {
                m_selectedAccount = value;
                NotifyPropertyChanged("SelectedAccount");
            }
        }

        public void Save()
        {
            if (SelectedAccount == null)
                return;

            using (var store = new DataStoreContext())
            {
                if (store.Options.Any())
                    store.CurrentOptions.SelectedAccountId = SelectedAccount.Id;
                else
                    store.Options.InsertOnSubmit(new Options { SelectedAccountId = SelectedAccount.Id });
                store.SubmitChanges();

                var account               = store.CurrentOptions.GetSelectedAccount(store);
                account.UseAvailableFunds = SelectedAccount.UseAvailableFunds;
                account.CreditLimit       = SelectedAccount.CreditLimit;

                store.SubmitChanges();
            }
        }

        public void SendErrorReport()
        {
            string message = "";
            using (var store = new DataStoreContext())
            {
                if (!store.Errors.Any())
                    return;

                message = store.Errors.ToArray().Select(x => x.ToString()).Aggregate((a, b) => a + "\n-----------\n\n" + b);

                store.Errors.DeleteAllOnSubmit(store.Errors);
                store.SubmitChanges();
            }
            var emailComposeTask = new EmailComposeTask
            {
                Subject = "Commonwealth Bank Error Report",
                Body    = message,
                To      = "dev@cpmcgrath.com",
            };
            
            emailComposeTask.Show();
        }
    }
}