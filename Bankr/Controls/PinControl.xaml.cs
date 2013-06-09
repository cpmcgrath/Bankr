using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CMcG.CommonwealthBank.Controls
{
    public partial class PinControl : UserControl
    {
        public PinControl()
        {
            InitializeComponent();
            MaxPasswordLength = 4;
            LayoutRoot.DataContext = new PinControlViewModel { Parent = this };
        }

        PinControlViewModel ViewModel
        {
            get { return (PinControlViewModel)LayoutRoot.DataContext; }
        }

        #region public string Password { get; set; }
        public static readonly DependencyProperty PasswordProperty = DependencyProperties<PinControl>.Register(p => p.Password, "");

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        #endregion

        public int MaxPasswordLength { get; set; }

        void OnNumberPressed(object sender, RoutedEventArgs e)
        {
            ViewModel.Password += (string)((Button)sender).Content;
        }

        void Clear(object sender, RoutedEventArgs e)
        {
            ViewModel.Password = "";
        }

        class PinControlViewModel : ViewModels.ViewModelBase
        {
            public PinControl Parent { get; set; }

            public string Password
            {
                get { return Parent.Password; }
                set
                {
                    Parent.Password = value;
                    NotifyPropertyChanged("Password", "MaskedPassword", "MaxLengthNotReached");
                }
            }

            public bool MaxLengthNotReached
            {
                get { return Password.Length < Parent.MaxPasswordLength; }
            }

            public string MaskedPassword
            {
                get { return "".PadLeft(Password.Length, '●'); }
            }
        }
    }
}
