using CMcG.CommonwealthBank.Data;
using CMcG.CommonwealthBank.Logic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CMcG.CommonwealthBank.ViewModels.Transfer
{
    public class FinishTransferViewModel : ViewModelBase
    {
        public FinishTransferViewModel(int fromAccountId, int toAccountId)
        {
            using (var store = new DataStoreContext())
            {
                FromAccount = store.Accounts.First(x => x.Id == fromAccountId);
                ToAccount   = store.TransferToAccounts.First(x => x.Id == toAccountId);
            }
        }

        public Account           FromAccount { get; private set; }
        public TransferToAccount ToAccount   { get; private set; }

        decimal m_amount;
        public decimal Amount
        {
            get { return m_amount; }
            set
            {
                m_amount = value;
                FirePropertyChanged();
            }
        }

        string m_yourDescription;
        public string YourDescription
        {
            get { return m_yourDescription; }
            set
            {
                m_yourDescription = value;
                FirePropertyChanged();
            }
        }

        string m_theirDescription;
        public string TheirDescription
        {
            get { return m_theirDescription ?? YourDescription; }
            set
            {
                m_theirDescription = value;
                FirePropertyChanged();
            }
        }

        public string[] DescriptionList
        {
            get
            {
                return new string[] { };
            }
        }

        public void MakeTransaction()
        {
            throw new NotImplementedException();
        }
    }
}