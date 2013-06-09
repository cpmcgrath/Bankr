using System;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace CMcG.Bankr.Data
{
    [Table]
    public partial class Options : NotifyBase
    {
        int  m_id;
        int  m_selectedAccountId;
        bool m_autoRefresh = true;

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
        public int SelectedAccountId
        {
            get { return m_selectedAccountId; }
            set
            {
                if (m_selectedAccountId != value)
                {
                    SendPropertyChanging();
                    m_selectedAccountId = value;
                    SendPropertyChanged("SelectedAccountId");
                }
            }
        }

        [Column(DbType = "BIT DEFAULT 1 NOT NULL")]
        public bool AutoRefresh
        {
            get { return m_autoRefresh; }
            set
            {
                if (m_autoRefresh != value)
                {
                    SendPropertyChanging();
                    m_autoRefresh = value;
                    SendPropertyChanged("AutoRefresh");
                }
            }
        }

        public Account GetSelectedAccount(DataStoreContext store)
        {
            return store.Accounts.FirstOrDefault(x => x.Id == SelectedAccountId);
        }
    }
}