using System;
using System.Linq;
using CMcG.Bankr.Data;
using Caliburn.Micro;

namespace CMcG.Bankr.ViewModels.Options
{
    public class ReplacementEditViewModel : ViewModelBase
    {
        public ReplacementEditViewModel(INavigationService navigationService) : base(navigationService)
        {
            TransactionId = -1;
            Id            = -1;
        }
        protected override void OnLoad()
        {
            if (Id >= 0)
                Load(Id);
            else
                Create(TransactionId);
        }

        public void Create(int transactionId)
        {
            Data = new Replacement();

            using (var store = new DataStoreContext())
            {
                var transaction = store.Transactions.First(x => x.Id == transactionId);
                Data.Original   = transaction.Summary.Replace("Sent to ", "").Replace("Received from ", "");
            }
        }

        public void Load(int id)
        {
            Existing = true;
            using (var store = new DataStoreContext())
            {
                Data = store.Replacements.First(x => x.Id == id);
            }
        }

        public bool        Existing      { get; private set; }
        public int         TransactionId { get; set; }
        public int         Id            { get; set; }
        public Replacement Data          { get; set; }

        public void Save()
        {
            using (var store = new DataStoreContext())
            {
                if (Existing)
                {
                    var data = store.Replacements.First(x => x.Id == Data.Id);
                    data.Original = Data.Original;
                    data.NewValue = Data.NewValue;
                }
                else
                    store.Replacements.InsertOnSubmit(Data);
                store.SubmitChanges();
                Existing = true;
            }
            Navigator.GoBack();
        }

        public void Delete()
        {
            if (!Existing)
                return;

            using (var store = new DataStoreContext())
            {
                var data = store.Replacements.First(x => x.Id == Data.Id);
                store.Replacements.DeleteOnSubmit(data);
                store.SubmitChanges();
            }
            Navigator.GoBack();
        }
    }
}
