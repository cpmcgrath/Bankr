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
            set { SetValue(ref m_id, value); }
        }

        [Column]
        public System.DateTime Time
        {
            get { return m_time; }
            set { SetValue(ref m_time, value); }
        }

        [Column(CanBeNull=false)]
        public string Message
        {
            get { return m_message; }
            set { SetValue(ref m_message, value); }
        }

        [Column(CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
        public string Extended
        {
            get { return m_extended; }
            set { SetValue(ref m_extended, value); }
        }

        public override string ToString()
        {
            return "----" + Time + "---------------------\n"
                 + "-Message:" + Message + "\n"
                 + "-Extended:" + Extended;
        }
    }
}