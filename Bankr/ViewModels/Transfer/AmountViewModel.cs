using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace CMcG.Bankr.ViewModels.Transfer
{
    [Description("3. Amount")]
    class AmountViewModel : ViewModels.ViewModelBase
    {
        public AmountViewModel(int fromAccountId, int toAccountId)
        {
            FromAccountId = fromAccountId;
            ToAccountId   = toAccountId;
        }
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
        public int FromAccountId { get; private set; }
        public int ToAccountId   { get; private set; }
    }
}