using System;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using CMcG.Bankr.Data;
using System.Collections.Generic;
using CMcG.Bankr.Logic;
using Caliburn.Micro;

namespace CMcG.Bankr.ViewModels.Transfer
{
    [Description("4. Transfer Money")]
    public class FinishTransferViewModel : ViewModelBase
    {
        public FinishTransferViewModel(INavigationService navigationService) : base(navigationService) { }

        protected override void OnLoad()
        {
            using (var store = new DataStoreContext())
            {
                FromAccount = store.Accounts.First(x => x.Id == FromAccountId);
                ToAccount   = store.TransferToAccounts.First(x => x.Id == ToAccountId);
            }
        }

        public int FromAccountId { get; set; }
        public int ToAccountId   { get; set; }

        public Account           FromAccount { get; set; }
        public TransferToAccount ToAccount   { get; set; }
        public decimal           Amount      { get; set; }

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

        public async void MakeTransaction()
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
                    Navigator.GoBack(4);
                }
            }
        }

        public void Cancel()
        {
            Navigator.GoBack(4);
        }
    }
}