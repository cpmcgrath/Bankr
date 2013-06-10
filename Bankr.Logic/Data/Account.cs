using System;
using System.Linq;
using Newtonsoft.Json;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Collections.Generic;
using CMcG.Bankr.Logic;
using Microsoft.Phone.Data.Linq.Mapping;

namespace CMcG.Bankr.Data
{
    [Index(Name="UQ__Account__000000000000006A", Columns="AccountName ASC", IsUnique=true)]
    [Table]
    public partial class Account : NotifyBase
    {
        int                    m_id;
        string                 m_accountName;
        string                 m_accountNumber;
        decimal                m_balance;
        decimal                m_availableFunds;
        bool                   m_useAvailableFunds;
        int                    m_creditLimit;
        DateTime               m_lastUpdate;
        EntitySet<Transaction> m_transactions;

        public Account()
        {
            m_transactions = new EntitySet<Transaction>(AttachTransactions, DetachTransactions);
        }

        [Column(IsPrimaryKey=true)]
        public int Id
        {
            get { return m_id; }
            set { SetValue(ref m_id, value); }
        }

        [Column(CanBeNull=false)]
        public string AccountName
        {
            get { return m_accountName; }
            set { SetValue(ref m_accountName, value); }
        }

        [Column(CanBeNull=false)]
        public string AccountNumber
        {
            get { return m_accountNumber; }
            set { SetValue(ref m_accountNumber, value); }
        }

        [Column, JsonConverter(typeof(CbaAmountConverter))]
        public decimal Balance
        {
            get { return m_balance; }
            set { SetValue(ref m_balance, value); }
        }

        [Column, JsonConverter(typeof(CbaAmountConverter))]
        public decimal AvailableFunds
        {
            get { return m_availableFunds; }
            set { SetValue(ref m_availableFunds, value); }
        }

        [Column]
        public DateTime LastUpdate
        {
            get { return m_lastUpdate; }
            set { SetValue(ref m_lastUpdate, value); }
        }

        [Column]
        public bool UseAvailableFunds
        {
            get { return m_useAvailableFunds; }
            set { SetValue(ref m_useAvailableFunds, value); }
        }

        [Column]
        public int CreditLimit
        {
            get { return m_creditLimit; }
            set { SetValue(ref m_creditLimit, value); }
        }

        public decimal AccountAmount
        {
            get
            {
                return UseAvailableFunds ? AvailableFunds - CreditLimit
                                         : Balance;
            }
        }

        [AssociationAttribute(Name="AccountTransaction", ThisKey="Id", OtherKey="AccountId", DeleteRule="NO ACTION")]
        public EntitySet<Transaction> TransactionList
        {
            get { return m_transactions; }
            set { m_transactions.Assign(value); }
        }

        public Data.Transaction[] Transactions { get; set; }

        private void AttachTransactions(Transaction entity)
        {
            FirePropertyChanging();
            entity.Account = this;
        }

        private void DetachTransactions(Transaction entity)
        {
            FirePropertyChanging();
            entity.Account = null;
        }
    }
}