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
            set { SetValue(ref m_id, value); }
        }

        [Column]
        public int SelectedAccountId
        {
            get { return m_selectedAccountId; }
            set { SetValue(ref m_selectedAccountId, value); }
        }

        [Column(DbType = "BIT DEFAULT 1 NOT NULL")]
        public bool AutoRefresh
        {
            get { return m_autoRefresh; }
            set { SetValue(ref m_autoRefresh, value); }
        }

        public Account GetSelectedAccount(DataStoreContext store)
        {
            return store.Accounts.FirstOrDefault(x => x.Id == SelectedAccountId);
        }
    }
}