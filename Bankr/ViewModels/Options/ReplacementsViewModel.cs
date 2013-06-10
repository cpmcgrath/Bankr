using System;
using System.Linq;
using CMcG.Bankr.Data;
using Caliburn.Micro;

namespace CMcG.Bankr.ViewModels.Options
{
    public class ReplacementsViewModel : ViewModelBase
    {
        public ReplacementsViewModel(INavigationService navigationService) : base(navigationService)
        {
            using (var store = new DataStoreContext())
            {
                Replacements = store.Replacements.OrderByDescending(x => x.Id).ToArray();
            }
        }

        public Replacement[] Replacements { get; set; }

        public void EditReplacement(int id)
        {
            Navigator.UriFor<ReplacementEditViewModel>().WithParam(p => p.Id, id).Navigate();
        }
    }
}
