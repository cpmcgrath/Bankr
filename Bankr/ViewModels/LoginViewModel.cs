using System;
using System.Linq;
using System.Collections.Generic;
using CMcG.CommonwealthBank.Data;
using CMcG.CommonwealthBank.Logic;

namespace CMcG.CommonwealthBank.ViewModels
{
    public class LoginViewModel
    {
        public LoginViewModel()
        {
            using (var store = new DataStoreContext())
                Username = store.LoginDetails.First().Username;

            Username = "xxxxx" + Username.Substring(Username.Length - 3);
        }

        public string Username { get; private set; }
        public string Password { get; set; }

        public bool Login()
        {
            using (var store = new DataStoreContext())
            {
                var hash   = store.LoginDetails.First().Password;
                var actual = new TwoWayEncryption().Decrypt(hash);
                return Password == actual;
            }
        }
    }
}
