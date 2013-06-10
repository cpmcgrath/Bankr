using System;
using System.Linq;
using System.Data.Linq.Mapping;

namespace CMcG.Bankr.Data
{
    [Table]
    public class Replacement : NotifyBase
    {
        int    m_id;
        string m_original = string.Empty;
        string m_newValue = string.Empty;

        [Column(AutoSync=AutoSync.OnInsert, IsPrimaryKey=true, IsDbGenerated=true)]
        public int Id
        {
            get { return m_id; }
            set { SetValue(ref m_id, value); }
        }

        [Column(CanBeNull=false)]
        public string Original
        {
            get { return m_original; }
            set { SetValue(ref m_original, value); }
        }

        [Column(CanBeNull=false)]
        public string NewValue
        {
            get { return m_newValue; }
            set { SetValue(ref m_newValue, value); }
        }
    }
}
