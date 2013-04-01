using System;
using System.Linq;
using System.Collections.Generic;

namespace CMcG.CommonwealthBank
{
    public class Security
    {
        Dictionary<Type, LoginType> m_secureItems = new Dictionary<Type, LoginType>();

        public bool IsLoggedIn { get; set; }
        public bool HasLogin
        {
            get
            {
                using (var store = new Data.DataStoreContext())
                    return store.LoginDetails.Any();
            }
        }

        public bool HasPin
        {
            get
            {
                using (var store = new Data.DataStoreContext())
                    return store.LoginDetails.Any() && !string.IsNullOrEmpty(store.LoginDetails.First().Pin);
            }
        }

        public LoginType LogonRequired<TViewModel>()
        {
            if (!HasLogin)
                return LoginType.CreateLogin;

            var permission = m_secureItems.ContainsKey(typeof(TViewModel)) ? m_secureItems[typeof(TViewModel)] : LoginType.None;

            if (!IsLoggedIn && (permission & LoginType.Password) == LoginType.Password)
                return LoginType.Password;

            if ((permission & LoginType.Pin) == LoginType.Pin)
                return HasPin ? LoginType.Pin : LoginType.Password;

            return LoginType.None;
        }

        public void UpdatePermission<TViewModel>(LoginType loginType)
        {
            bool alreadySecure = m_secureItems.ContainsKey(typeof(TViewModel));

            if (loginType == LoginType.None)
                m_secureItems.Remove(typeof(TViewModel));
            else if (alreadySecure)
                m_secureItems[typeof(TViewModel)] = loginType;
            else
                m_secureItems.Add(typeof(TViewModel), loginType);
        }

        [Flags]
        public enum LoginType
        {
            None,
            Pin,
            Password,
            PinAndPassword = Pin | Password,
            CreateLogin
        }
    }
}
