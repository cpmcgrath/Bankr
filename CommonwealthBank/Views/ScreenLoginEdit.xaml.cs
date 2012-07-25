using System;
using System.Linq;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using CMcG.CommonwealthBank.ViewModels;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace CMcG.CommonwealthBank.Views
{
    public partial class ScreenLoginEdit : PhoneApplicationPage
    {
        public ScreenLoginEdit()
        {
            InitializeComponent();
            DataContext = new LoginEditViewModel();
        }

        public Action OnSave { get; set; }

        void Save(object sender, RoutedEventArgs e)
        {
            var vm = (LoginEditViewModel)DataContext;
            vm.Save();

            var popup = (Popup)Parent;
            popup.IsOpen = false;
            OnSave();
        }
    }
}