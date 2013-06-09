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
            set
            {
                if (m_id == value)
                    return;

                SendPropertyChanging();
                m_id = value;
                SendPropertyChanged("Id");
            }
        }

        [Column(CanBeNull=false)]
        public string AccountName
        {
            get { return m_accountName; }
            set
            {
                if (m_accountName == value)
                    return;

                SendPropertyChanging();
                m_accountName = value;
                SendPropertyChanged("AccountName");
            }
        }

        [Column(CanBeNull=false)]
        public string AccountNumber
        {
            get { return m_accountNumber; }
            set
            {
                if (m_accountNumber == value)
                    return;

                SendPropertyChanging();
                m_accountNumber = value;
                SendPropertyChanged("AccountNumber");
            }
        }

        [Column, JsonConverter(typeof(CbaAmountConverter))]
        public decimal Balance
        {
            get { return m_balance; }
            set
            {
                if (m_balance == value)
                    return;

                SendPropertyChanging();
                m_balance = value;
                SendPropertyChanged("Balance");
            }
        }

        [Column, JsonConverter(typeof(CbaAmountConverter))]
        public decimal AvailableFunds
        {
            get { return m_availableFunds; }
            set
            {
                if (m_availableFunds == value)
                    return;

                SendPropertyChanging();
                m_availableFunds = value;
                SendPropertyChanged("AvailableFunds");
            }
        }

        [Column]
        public DateTime LastUpdate
        {
            get { return m_lastUpdate; }
            set
            {
                if (m_lastUpdate == value)
                    return;

                SendPropertyChanging();
                m_lastUpdate = value;
                SendPropertyChanged("LastUpdate");
            }
        }

        [Column]
        public bool UseAvailableFunds
        {
            get { return m_useAvailableFunds; }
            set
            {
                if (m_useAvailableFunds == value)
                    return;

                SendPropertyChanging();
                m_useAvailableFunds = value;
                SendPropertyChanged("UseAvailableFunds");
            }
        }

        [Column]
        public int CreditLimit
        {
            get { return m_creditLimit; }
            set
            {
                if (m_creditLimit == value)
                    return;

                SendPropertyChanging();
                m_creditLimit = value;
                SendPropertyChanged("CreditLimit");
            }
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
            SendPropertyChanging();
            entity.Account = this;
        }

        private void DetachTransactions(Transaction entity)
        {
            SendPropertyChanging();
            entity.Account = null;
        }
    }
}