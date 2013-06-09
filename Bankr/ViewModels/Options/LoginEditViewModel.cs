using System;
using System.Linq;
using System.Collections.Generic;
using CMcG.Bankr.Data;
using CMcG.Bankr.Logic;
using System.Security.Cryptography;

namespace CMcG.Bankr.ViewModels.Options
{
    public class LoginEditViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public void Save()
        {
            var encryptedPassword = new TwoWayEncryption().Encrypt(Password);
            var details = new LoginDetails { Username = Username, Password = encryptedPassword };
            using (var store = new DataStoreContext())
            {
                store.LoginDetails.InsertOnSubmit(details);
                store.SubmitChanges();
            }
        }
    }
}
