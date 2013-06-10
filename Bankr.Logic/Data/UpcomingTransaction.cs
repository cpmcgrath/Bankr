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
            set { SetValue(ref m_id, value); }
        }

        [Column, JsonConverter(typeof(CbaDateConverter))]
        public DateTime EffectiveDate
        {
            get { return m_date; }
            set { SetValue(ref m_date, value); }
        }

        [Column(CanBeNull=false)]
        public string TransactionType
        {
            get { return m_transactionType.Replace(@"<sup>rd</sup>", "rd"); }
            set { SetValue(ref m_transactionType, value); }
        }

        [Column]
        public string Status
        {
            get { return m_status; }
            set { SetValue(ref m_status, value); }
        }

        [Column, JsonConverter(typeof(CbaAmountConverter))]
        public decimal Amount
        {
            get { return m_amount; }
            set { SetValue(ref m_amount, value); }
        }

        [Column]
        public string TransactionFrom
        {
            get { return m_transactionFrom; }
            set { SetValue(ref m_transactionFrom, value); }
        }

        [Column]
        public string TransactionTo
        {
            get { return m_transactionTo; }
            set { SetValue(ref m_transactionTo, value); }
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