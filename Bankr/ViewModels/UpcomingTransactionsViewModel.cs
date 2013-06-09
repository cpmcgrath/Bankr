using CMcG.CommonwealthBank.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMcG.CommonwealthBank.ViewModels
{
    public class UpcomingTransactionsViewModel : ViewModelBase
    {
        public UpcomingTransactionsViewModel()
        {
            Load();
        }

        void Load()
        {
            using (var store = new DataStoreContext())
            {
                Transactions = store.UpcomingTransactions.OrderBy(x => x.EffectiveDate).ToArray();
            }
            NotifyPropertyChanged("Transactions");
        }

        public UpcomingTransaction[] Transactions { get; set; }
    }
}
