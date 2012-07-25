using System;
using System.Linq;
using CMcG.CommonwealthBank.Data;

namespace CMcG.CommonwealthBank.ViewModels
{
    public class ReplacementEditViewModel : ViewModelBase
    {
        public ReplacementEditViewModel(int transactionId)
        {
            Data = new Replacement();
            using (var store = new DataStoreContext())
            {
                var transaction = store.Transactions.First(x => x.Id == transactionId);
                Data.Original   = transaction.Summary.Replace("Sent to ", "").Replace("Received from ", "");
            }
        }

        public Replacement Data { get; set; }

        public void Save()
        {
            using (var store = new DataStoreContext())
            {
                store.Replacements.InsertOnSubmit(Data);
                store.SubmitChanges();
            }
        }
    }
}
