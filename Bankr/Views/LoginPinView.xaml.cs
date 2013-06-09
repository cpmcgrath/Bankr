using System;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using CMcG.Bankr.ViewModels;
using System.Windows.Controls.Primitives;

namespace CMcG.Bankr.Views
{
    public partial class LoginPinView : PhoneApplicationPage
    {
        public LoginPinView()
        {
            InitializeComponent();
            DataContext = new LoginPinViewModel();
        }

        public Action OnLogin { get; set; }

        void Login(object sender, RoutedEventArgs e)
        {
            var vm = (LoginPinViewModel)DataContext;
            if (vm.Login())
            {
                var popup = (Popup)Parent;
                popup.IsOpen = false;
                OnLogin();
            }
        }

        void SwitchToPassword(object sender, RoutedEventArgs e)
        {
            var passwordScreen = new LoginView { OnLogin = OnLogin, Width = Width, Height = Height };
            var popup = (Popup)Parent;
            popup.IsOpen = false;
            popup.Child = passwordScreen;
            popup.IsOpen = true;
        }
    }
}