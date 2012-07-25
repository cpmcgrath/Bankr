using System;
using System.Linq;
using System.Collections.Generic;

namespace CMcG.CommonwealthBank
{
    public class Security
    {
        List<Type> m_secureItems = new List<Type>();
        public bool IsLoggedIn { get; set; }
        public bool HasLogin
        {
            get
            {
                using (var store = new Data.DataStoreContext())
                    return store.LoginDetails.Any();
            }
        }

        public bool HasPermission<TViewModel>()
        {
            return HasLogin && (IsLoggedIn || !m_secureItems.Contains(typeof(TViewModel)));
        }

        public void UpdatePermission<TViewModel>(bool isSecure)
        {
            bool alreadySecure = m_secureItems.Contains(typeof(TViewModel));

            if (alreadySecure == isSecure)
                return;

            if (isSecure)
                m_secureItems.Add(typeof(TViewModel));
            else
                m_secureItems.Remove(typeof(TViewModel));
        }
    }
}
