using System;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using CMcG.Bankr.Data;
using CMcG.Bankr.ViewModels.Options;

namespace CMcG.Bankr.Views.Options
{
    public partial class ReplacementsView : PhoneApplicationPage
    {
        public ReplacementsView()
        {
            InitializeComponent();
        }

        void ViewReplacement(object sender, EventArgs e)
        {
            var ctl         = (FrameworkElement)sender;
            var replacement = (Replacement)ctl.DataContext;

            var vm = (ReplacementsViewModel)DataContext;
            vm.EditReplacement(replacement.Id);
        }
    }
}