using System;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Collections.Generic;
using CMcG.Bankr.Logic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text.RegularExpressions;
using System.Globalization;

namespace CMcG.Bankr.Data
{
    [Table]
    public partial class UpcomingTransaction : NotifyBase
    {
        int                m_id;
        DateTime           m_date;
        string             m_transactionType;
        string             m_status;
        decimal            m_amount;
        string             m_transactionFrom;
        string             m_transactionTo;

        [Column(AutoSync=AutoSync.OnInsert, IsPrimaryKey=true, IsDbGenerated=true)]
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

        [Column, JsonConverter(typeof(CbaDateConverter))]
        public DateTime EffectiveDate
        {
            get { return m_date; }
            set
            {
                if (m_date == value)
                    return;

                SendPropertyChanging();
                m_date = value;
                SendPropertyChanged("Date");
            }
        }

        [Column(CanBeNull=false)]
        public string TransactionType
        {
            get { return m_transactionType.Replace(@"<sup>rd</sup>", "rd"); }
            set
            {
                if (m_transactionType == value)
                    return;

                SendPropertyChanging();
                m_transactionType = value;
                SendPropertyChanged("TransactionType");
            }
        }

        [Column]
        public string Status
        {
            get { return m_status; }
            set
            {
                if (m_status == value)
                    return;

                SendPropertyChanging();
                m_status = value;
                SendPropertyChanged("Status");
            }
        }

        [Column, JsonConverter(typeof(CbaAmountConverter))]
        public decimal Amount
        {
            get { return m_amount; }
            set
            {
                if (m_amount == value)
                    return;

                SendPropertyChanging();
                m_amount = value;
                SendPropertyChanged("Amount");
            }
        }

        [Column]
        public string TransactionFrom
        {
            get { return m_transactionFrom; }
            set
            {
                if (m_transactionFrom == value)
                    return;

                SendPropertyChanging();
                m_transactionFrom = value;
                SendPropertyChanged("TransactionFrom");
            }
        }

        [Column]
        public string TransactionTo
        {
            get { return m_transactionTo; }
            set
            {
                if (m_transactionTo == value)
                    return;

                SendPropertyChanging();
                m_transactionTo = value;
                SendPropertyChanged("TransactionTo");
            }
        }

        public decimal AbsAmount
        {
            get { return Math.Abs(Amount); }
        }

        public bool Equals(UpcomingTransaction other)
        {
            return EffectiveDate   == other.EffectiveDate
                && TransactionType == other.TransactionType
                && Amount          == other.Amount
                && Status          == other.Status
                && TransactionFrom == other.TransactionFrom
                && TransactionTo   == other.TransactionTo;
        }

        public override string ToString()
        {
            return "TransactionType: " + TransactionType + ", Amount: " + Amount + ", Date:" + EffectiveDate + ". Status: " + Status;
        }

        public string Summary
        {
            get { return string.Format("{0:dd MMMM} {1} ({2:c})", EffectiveDate, TransactionTo, AbsAmount); }
        }
    }
}