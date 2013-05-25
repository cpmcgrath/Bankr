using System;
using System.Linq;
using Newtonsoft.Json;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Collections.Generic;
using CMcG.CommonwealthBank.Logic;
using Microsoft.Phone.Data.Linq.Mapping;

namespace CMcG.CommonwealthBank.Data
{
    [Table]
    public partial class TransferToAccount : NotifyBase
    {
        int         m_id;
        string      m_accountName;
        string      m_accountNumber;
        decimal     m_availableFunds;
        DateTime    m_lastUsed;
        AccountType m_type;

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

        public string MoreInformation
        {
            get
            {
                return Type == AccountType.ThirdParty
                     ? AccountNumber
                     : AvailableFunds.ToString("c") + "\r\n" + AccountNumber;
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
        public DateTime LastUsed
        {
            get { return m_lastUsed; }
            set
            {
                if (m_lastUsed == value)
                    return;

                SendPropertyChanging();
                m_lastUsed = value;
                SendPropertyChanged("LastUpdate");
            }
        }

        [Column]
        public AccountType Type
        {
            get { return m_type; }
            set
            {
                if (m_type == value)
                    return;

                SendPropertyChanging();
                m_type = value;
                SendPropertyChanged("Type");
            }
        }

        public enum AccountType
        {
            Linked     = 1,
            ThirdParty = 2
        }
    }
}