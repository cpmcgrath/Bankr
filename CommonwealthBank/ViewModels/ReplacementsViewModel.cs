using System;
using System.Linq;
using CMcG.CommonwealthBank.Data;

namespace CMcG.CommonwealthBank.ViewModels
{
    public class ReplacementsViewModel : ViewModelBase
    {
        public ReplacementsViewModel()
        {
            using (var store = new DataStoreContext())
            {
                Replacements = store.Replacements.OrderByDescending(x => x.Id).ToArray();
            }
        }

        public Replacement[] Replacements { get; set; }
    }
}
