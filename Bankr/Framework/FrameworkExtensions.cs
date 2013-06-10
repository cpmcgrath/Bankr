using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using System.Windows;
using System.Windows.Controls.Primitives;
using CMcG.Bankr.Data;
using Caliburn.Micro;
using Action = System.Action;

namespace CMcG.Bankr
{
    public static class FrameworkExtensions
    {
        static Popup s_popup;

        public static void CheckPermissions(Type viewModel, INavigationService navigationService, Action callback)
        {
            if (s_popup != null && s_popup.IsOpen)
                s_popup.IsOpen = false;

            PhoneApplicationPage screen = null;
            PhoneApplicationPage instance = (PhoneApplicationPage)navigationService.CurrentContent;

            switch (App.Current.Security.LogonRequired(viewModel))
            {
                case AccessLevel.None       : callback.Invoke(); return;
                case AccessLevel.CreateLogin: screen = new Views.Options.LoginEditView { OnSave  = () => ResetLook(instance, callback) }; break;
                case AccessLevel.Password   : screen = new Views.LoginView             { OnLogin = () => ResetLook(instance, callback) }; break;
                case AccessLevel.Pin        : screen = new Views.LoginPinView          { OnLogin = () => ResetLook(instance, callback, false) }; break;
            }

            Hide(instance);
            screen.Height =  App.Current.Host.Content.ActualHeight - 50;
            screen.Width  =  App.Current.Host.Content.ActualWidth;
            s_popup = new Popup
            {
                Child          = screen,
                VerticalOffset = 30,
                IsOpen         = true,
            };
        }

        static void Hide(PhoneApplicationPage instance)
        {
            instance.Opacity = 0;
            instance.BackKeyPress += HidePopup;

            if (instance.ApplicationBar != null)
                instance.ApplicationBar.IsVisible = false;
        }

        static void HidePopup(object s, System.ComponentModel.CancelEventArgs e)
        {
            var sender = (PhoneApplicationPage)s;
            s_popup.IsOpen = false;
            sender.BackKeyPress -= HidePopup;
        }

        static void ResetLook(PhoneApplicationPage instance, Action callback, bool isLoggedIn = true)
        {
            instance.BackKeyPress -= HidePopup;
            App.Current.Security.IsLoggedIn = true;
            instance.Opacity = 1;

            if (instance.ApplicationBar != null)
                instance.ApplicationBar.IsVisible = true;

            callback.Invoke();
        }
    }
}
