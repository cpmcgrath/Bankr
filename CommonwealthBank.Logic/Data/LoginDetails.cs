using System;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Collections.Generic;

namespace CMcG.CommonwealthBank.Data
{
    [Table]
    public partial class LoginDetails : NotifyBase
    {
        int    m_id;
        string m_username = string.Empty;
        string m_password = string.Empty;

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
        public string Username
        {
            get { return m_username; }
            set
            {
                if (m_username == value)
                    return;

                SendPropertyChanging();
                m_username = value;
                SendPropertyChanged("Username");
            }
        }

        [Column(CanBeNull=false)]
        public string Password
        {
            get { return m_password; }
            set
            {
                if (m_password == value)
                    return;

                SendPropertyChanging();
                m_password = value;
                SendPropertyChanged("Password");
            }
        }
    }
}