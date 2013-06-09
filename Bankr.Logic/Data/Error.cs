using System;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Collections.Generic;

namespace CMcG.Bankr.Data
{
    [Table]
    public partial class Error : NotifyBase
    {
        int      m_id;
        DateTime m_time;
        string   m_message;
        string   m_extended;

        [Column(AutoSync=AutoSync.OnInsert, IsPrimaryKey=true, IsDbGenerated=true)]
        public int Id
        {
            get { return m_id; }
            set
            {
                if (m_id != value)
                {
                    SendPropertyChanging();
                    m_id = value;
                    SendPropertyChanged("Id");
                }
            }
        }

        [Column]
        public System.DateTime Time
        {
            get { return m_time; }
            set
            {
                if (m_time != value)
                {
                    SendPropertyChanging();
                    m_time = value;
                    SendPropertyChanged("Time");
                }
            }
        }

        [Column(CanBeNull=false)]
        public string Message
        {
            get { return m_message; }
            set
            {
                if (m_message != value)
                {
                    SendPropertyChanging();
                    m_message = value;
                    SendPropertyChanged("Message");
                }
            }
        }

        [Column(CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
        public string Extended
        {
            get { return m_extended; }
            set
            {
                if (m_extended != value)
                {
                    SendPropertyChanging();
                    m_extended = value;
                    SendPropertyChanged("Extended");
                }
            }
        }

        public override string ToString()
        {
            return "----" + Time + "---------------------\n"
                 + "-Message:" + Message + "\n"
                 + "-Extended:" + Extended;
        }
    }
}