using System;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Collections.Generic;

namespace CMcG.Bankr.Data
{
    [Table]
    public partial class LoginDetails : NotifyBase
    {
        int    m_id;
        string m_username = string.Empty;
        string m_password = string.Empty;
        string m_pin      = string.Empty;

        [Column(AutoSync=AutoSync.OnInsert, IsPrimaryKey=true, IsDbGenerated=true)]
        public int Id
        {
            get { return m_id; }
            set { SetValue(ref m_id, value); }
        }

        [Column(CanBeNull=false)]
        public string Username
        {
            get { return m_username; }
            set { SetValue(ref m_username, value); }
        }

        [Column(CanBeNull=false)]
        public string Password
        {
            get { return m_password; }
            set { SetValue(ref m_password, value); }
        }

        [Column]
        public string Pin
        {
            get { return m_pin ?? string.Empty; }
            set { SetValue(ref m_pin, value); }
        }
    }
}