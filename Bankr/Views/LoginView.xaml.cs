using System;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using CMcG.CommonwealthBank.ViewModels;
using System.Windows.Controls.Primitives;

namespace CMcG.CommonwealthBank.Views
{
    public partial class LoginView : PhoneApplicationPage
    {
        public LoginView()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }

        public Action OnLogin { get; set; }

        void Login(object sender, RoutedEventArgs e)
        {
            var vm = (LoginViewModel)DataContext;
            if (vm.Login())
            {
                var popup = (Popup)Parent;
                popup.IsOpen = false;
                OnLogin();
            }
        }
    }
}