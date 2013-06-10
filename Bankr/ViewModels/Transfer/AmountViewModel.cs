using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using Caliburn.Micro;

namespace CMcG.Bankr.ViewModels.Transfer
{
    [Description("3. Amount")]
    public class AmountViewModel : ViewModels.ViewModelBase
    {
        public AmountViewModel(INavigationService navigationService) : base(navigationService) { }

        string m_amount = "$";
        public string Amount
        {
            get { return m_amount; }
            set
            {
                m_amount = value;
                NotifyPropertyChanged("Amount", "CanInsertDecimalPoint", "CanInsertNumber", "CanInsertZero");
            }
        }

        public bool CanInsertZero
        {
            get { return Amount != "$0"; }
        }
        public bool CanInsertDecimalPoint
        {
            get { return Amount != "$" && !Amount.Contains("."); }
        }

        public bool CanInsertNumber
        {
            get { return !Amount.Contains(".") || Amount.Substring(Amount.IndexOf(".")).Length < 3; }
        }

        public decimal Value
        {
            get { return int.Parse(Amount.Substring(1)); }
        }

        public int FromAccountId { get; set; }
        public int ToAccountId   { get; set; }

        public void GoToConfirmation()
        {
            SelectAmount(Value);
        }

        public void SelectAmount(decimal amount)
        {
            Navigator.UriFor<FinishTransferViewModel>()
                     .WithParam(p => p.FromAccountId, FromAccountId)
                     .WithParam(p => p.ToAccountId,   ToAccountId)
                     .WithParam(p => p.Amount,        amount)
                     .Navigate();
        }

        public void Cancel()
        {
            Navigator.GoBack(3);
        }
    }
}