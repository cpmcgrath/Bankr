using System;
using System.Linq;
using System.Collections.Generic;
using CMcG.CommonwealthBank.Data;
using CMcG.CommonwealthBank.Logic;

namespace CMcG.CommonwealthBank.ViewModels
{
    public class LoginPinViewModel
    {
        public string Pin { get; set; }

        public bool Login()
        {
            using (var store = new DataStoreContext())
            {
                var hash   = store.LoginDetails.First().Pin;
                var actual = new TwoWayEncryption().Decrypt(hash);
                return Pin == actual;
            }
        }
    }
}
