using System;
using System.Linq;
using System.Collections.Generic;
using CMcG.CommonwealthBank.Data;
using CMcG.CommonwealthBank.Logic;
using System.Security.Cryptography;

namespace CMcG.CommonwealthBank.ViewModels
{
    public class LoginEditViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public void Save()
        {
            var encryptedPassword = new TwoWayEncryption().Encrypt(Password);
            var details = new LoginDetails { Username = Username, Password = encryptedPassword };
            var store = new DataStoreContext();
            store.LoginDetails.InsertOnSubmit(details);
            store.SubmitChanges();
        }
    }
}
