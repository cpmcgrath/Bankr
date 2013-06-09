using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using CMcG.CommonwealthBank.Data;
using CMcG.CommonwealthBank.ViewModels.Transfer;
using CMcG.CommonwealthBank.ViewModels.Options;

namespace CMcG.CommonwealthBank
{
    public class Security
    {
        Dictionary<Type, AccessLevel> m_secureItems = new Dictionary<Type, AccessLevel>();

        public void LoadFromDatabase()
        {
            m_secureItems = new Dictionary<Type,AccessLevel>();
            using (var store = new DataStoreContext())
            {
                if (!store.ScreenSecurity.Any())
                {
                    UpdatePermission<OptionsViewModel    >(AccessLevel.PinAndPassword);
                    UpdatePermission<PickAccountViewModel>(AccessLevel.Pin);
                }
                else
                {
                    foreach (var screen in store.ScreenSecurity)
                        UpdatePermission(screen.ViewModelType, screen.AccessLevel);
                }
            }
        }

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

        public AccessLevel LogonRequired<TViewModel>()
        {
            return LogonRequired(typeof(TViewModel));
        }
        public AccessLevel LogonRequired(Type viewModel)
        {
            if (!HasLogin)
                return AccessLevel.CreateLogin;

            var permission = m_secureItems.ContainsKey(viewModel) ? m_secureItems[viewModel] : AccessLevel.None;

            if (!IsLoggedIn && (permission & AccessLevel.Password) == AccessLevel.Password)
                return AccessLevel.Password;

            if ((permission & AccessLevel.Pin) == AccessLevel.Pin)
                return HasPin ? AccessLevel.Pin : AccessLevel.Password;

            return AccessLevel.None;
        }

        public void UpdatePermission<TViewModel>(AccessLevel loginType)
        {
            UpdatePermission(typeof(TViewModel), loginType);
        }
        public void UpdatePermission(Type vm, AccessLevel loginType)
        {
            bool alreadySecure = m_secureItems.ContainsKey(vm);

            if (loginType == AccessLevel.None)
                m_secureItems.Remove(vm);
            else if (alreadySecure)
                m_secureItems[vm] = loginType;
            else
                m_secureItems.Add(vm, loginType);
        }

        public AccessLevel GetAccessLevel(Type vm)
        {
            return m_secureItems.ContainsKey(vm) ? m_secureItems[vm] : AccessLevel.None;
        }
    }
}
