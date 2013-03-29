using System;
using System.Linq;
using CMcG.CommonwealthBank.Data;

namespace CMcG.CommonwealthBank.ViewModels.Options
{
    public class ReplacementEditViewModel : ViewModelBase
    {
        public bool Existing { get; private set; }
        private ReplacementEditViewModel()
        {
        }

        public static ReplacementEditViewModel Create(int transactionId)
        {
            var item  = new ReplacementEditViewModel();
            item.Data = new Replacement();

            using (var store = new DataStoreContext())
            {
                var transaction = store.Transactions.First(x => x.Id == transactionId);
                item.Data.Original   = transaction.Summary.Replace("Sent to ", "").Replace("Received from ", "");
            }
            return item;
        }

        public static ReplacementEditViewModel Load(int id)
        {
            var item = new ReplacementEditViewModel { Existing = true };
            using (var store = new DataStoreContext())
            {
                item.Data = store.Replacements.First(x => x.Id == id);
            }
            return item;
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
