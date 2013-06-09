using System;
using System.Linq;
using System.Collections.Generic;
using CMcG.Bankr.Data;
using CMcG.Bankr.Logic;

namespace CMcG.Bankr.ViewModels
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
