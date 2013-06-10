using System;
using System.Linq;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using CMcG.Bankr.ViewModels.Options;

namespace CMcG.Bankr.Views.Options
{
    public partial class OptionsView : PhoneApplicationPage
    {
        public OptionsView()
        {
            InitializeComponent();
        }

        void Save(object sender, EventArgs e)
        {
            this.FinishBinding();
            ((OptionsViewModel)DataContext).Save();
        }
    }
}