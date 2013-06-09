using System;
using System.Linq;
using CMcG.Bankr.Data;

namespace CMcG.Bankr.ViewModels.Options
{
    public class ReplacementEditViewModel : ViewModelBase
    {
        public bool Existing { get; private set; }
        public ReplacementEditViewModel(int id, int transactionId = -1)
        {
            if (id >= 0)
                Load(id);
            else
                Create(transactionId);
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

        public Replacement Data { get; set; }

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
        }
    }
}
