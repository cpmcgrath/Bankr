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
    [Table]
    public partial class TransferToAccount : NotifyBase
    {
        int         m_id;
        int?        m_senderId;
        string      m_accountName;
        string      m_accountNumber;
        decimal     m_availableFunds;
        DateTime    m_lastUsed;
        AccountType m_type;

        [Column(IsPrimaryKey=true)]
        public int Id
        {
            get { return m_id; }
            set { SetValue(ref m_id, value); }
        }

        [Column]
        public int? SenderId
        {
            get { return m_senderId; }
            set { SetValue(ref m_senderId, value); }
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
            set { SetValue(ref m_availableFunds, value); }
        }

        [Column]
        public DateTime LastUsed
        {
            get { return m_lastUsed; }
            set { SetValue(ref m_lastUsed, value); }
        }

        [Column]
        public AccountType Type
        {
            get { return m_type; }
            set { SetValue(ref m_type, value); }
        }

        public enum AccountType
        {
            Linked     = 1,
            ThirdParty = 2
        }
    }
}