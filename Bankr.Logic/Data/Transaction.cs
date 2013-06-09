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
    public partial class Transaction : NotifyBase
    {
        int                m_id;
        DateTime           m_date;
        string             m_description;
        bool               m_isPending;
        decimal            m_amount;
        int                m_accountId;
        bool               m_isSeen;
        EntityRef<Account> m_account = default(EntityRef<Account>);

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
        public string Description
        {
            get { return m_description; }
            set
            {
                if (m_description == value)
                    return;

                SendPropertyChanging();
                m_description = value;
                SendPropertyChanged("Description");
            }
        }

        [Column]
        public bool IsPending
        {
            get { return m_isPending; }
            set
            {
                if (m_isPending == value)
                    return;

                SendPropertyChanging();
                m_isPending = value;
                SendPropertyChanged("IsPending");
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
        public int AccountId
        {
            get { return m_accountId; }
            set
            {
                if (m_accountId == value)
                    return;

                if (m_account.HasLoadedOrAssignedValue)
                    throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();

                SendPropertyChanging();
                m_accountId = value;
                SendPropertyChanged("AccountId");
            }
        }

        [Column]
        public bool IsSeen
        {
            get { return m_isSeen; }
            set
            {
                if (m_isSeen == value)
                    return;

                SendPropertyChanging();
                m_isSeen = value;
                SendPropertyChanged("IsSeen");
            }
        }

        [Association(Name="AccountTransaction", Storage="m_account", ThisKey="AccountId", OtherKey="Id", IsForeignKey=true)]
        public Account Account
        {
            get { return m_account.Entity; }
            set
            {
                Account previousValue = m_account.Entity;
                if (previousValue == value && m_account.HasLoadedOrAssignedValue)
                    return;

                SendPropertyChanging();
                if (previousValue != null)
                {
                    m_account.Entity = null;
                    previousValue.TransactionList.Remove(this);
                }
                m_account.Entity = value;

                if (value != null)
                    value.TransactionList.Add(this);

                m_accountId = value != null ? value.Id : 0;

                SendPropertyChanged("Account");
            }
        }

        public Replacement[] Replacements { get; set; }

        public string Summary
        {
            get
            {
                var desc = Description.Replace("<br/>",                           " ")
                                      .Replace(@"\s+",                            m => " ")
                                      .RemoveAllCaps()
                                      .Replace(" Kaching",                        ":")
                                      .Replace("Transfer to ",                    "")
                                      .Replace("Transfer from ",                  "")
                                      .Replace("Qld Au",                          "")
                                      .Replace("Ql Aus",                          "")
                                      .Replace(@"Direct Debit \d+",               m => "DD")
                                      .Replace(@"Card xx\d\d\d\d",                m => "")
                                      .Replace(@"Value Date: \d\d/\d\d/\d\d\d\d", m => "");

                if (Replacements != null)
                    foreach (var replacement in Replacements)
                        desc = desc.Replace(replacement.Original, replacement.NewValue);

                var format = Amount < 0 ? "Sent to {1}"
                                        : "Received from {1}";
                return string.Format(format, AbsAmount, desc);
            }
        }

        public decimal AbsAmount
        {
            get { return Math.Abs(Amount); }
        }

        public DateTime? DateOfPurchase
        {
            get
            {
                var match = Regex.Match(Description, @"Value Date: (?<Date>\d\d/\d\d/\d\d\d\d)");
                return match.Success ? DateTime.ParseExact(match.Groups["Date"].Value, "dd/MM/yyyy", CultureInfo.CurrentCulture)
                                     : (DateTime?)null;
            }
        }

        public bool Equals(Transaction other)
        {
            return EffectiveDate == other.EffectiveDate
                && Description   == other.Description
                && Amount        == other.Amount
                && IsPending     == other.IsPending;
        }

        public override string ToString()
        {
            return "Desc: " + Description + ", Amount: " + Amount + ", Date:" + EffectiveDate + ". Pending: " + IsPending;
        }
    }
}