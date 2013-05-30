using CMcG.CommonwealthBank.Data;
using CMcG.CommonwealthBank.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CMcG.CommonwealthBank.ViewModels.Transfer
{
    public class FinishTransferViewModel : ViewModelBase
    {
        public FinishTransferViewModel(int fromAccountId, int toAccountId, decimal amount)
        {
            Amount = amount;
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

        string m_description;
        public string Description
        {
            get { return m_description; }
            set
            {
                m_description = value;
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

        public async void MakeTransaction(Navigator navigator)
        {
            var retriever = new DataRetriever { Status = CurrentApp.Status };
            if (retriever.CanTransferMoney)
            {
                var receipt = await retriever.TransferMoney(FromAccount.Id, ToAccount.Id, Description, Amount);

                if (string.IsNullOrEmpty(receipt))
                    MessageBox.Show("Transfer failed");
                else
                {
                    MessageBox.Show("Money Transfered.\r\nReceipt:" + receipt);
                    navigator.GoBack(3);
                }
            }
        }
    }
}