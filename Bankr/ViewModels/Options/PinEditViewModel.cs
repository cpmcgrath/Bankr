using System;
using System.Linq;
using System.Collections.Generic;
using CMcG.Bankr.Data;
using CMcG.Bankr.Logic;
using Caliburn.Micro;

namespace CMcG.Bankr.ViewModels.Options
{
    public class PinEditViewModel : ViewModelBase
    {
        public PinEditViewModel(INavigationService navigationService) : base(navigationService) { }
        public string Pin { get; set; }

        public void Save()
        {
            var encryptedPin = new TwoWayEncryption().Encrypt(Pin);
            using (var store = new DataStoreContext())
            {
                var details = store.LoginDetails.First();
                details.Pin = encryptedPin;

                store.SubmitChanges();
            }

            Navigator.GoBack();
        }
    }
}
