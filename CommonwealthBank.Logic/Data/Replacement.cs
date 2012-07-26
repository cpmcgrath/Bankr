using System;
using System.Linq;
using System.Data.Linq.Mapping;

namespace CMcG.CommonwealthBank.Data
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
        public string Original
        {
            get { return m_original; }
            set
            {
                if (m_original == value)
                    return;

                SendPropertyChanging();
                m_original = value;
                SendPropertyChanged("Original");
            }
        }

        [Column(CanBeNull=false)]
        public string NewValue
        {
            get { return m_newValue; }
            set
            {
                if (m_newValue == value)
                    return;

                SendPropertyChanging();
                m_newValue = value;
                SendPropertyChanged("NewValue");
            }
        }
    }
}
