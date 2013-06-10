using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Phone.Controls;
using System.Windows.Controls.Primitives;
using CMcG.Bankr.Data;
using Caliburn.Micro;
using Action = System.Action;

namespace CMcG.Bankr
{
    public class PermissionChecker
    {
        public PermissionChecker(Type viewModel, INavigationService navigator, Action callback)
        {
            var instance = (PhoneApplicationPage)navigator.CurrentContent;
            m_viewModel  = viewModel;
            m_callback   = callback;

            switch (App.Current.Security.LogonRequired(m_viewModel))
            {
                case AccessLevel.None       : return;
                case AccessLevel.CreateLogin: m_screen = new Views.Options.LoginEditView { OnSave  = () => ResetLook(instance, callback) }; break;
                case AccessLevel.Password   : m_screen = new Views.LoginView             { OnLogin = () => ResetLook(instance, callback) }; break;
                case AccessLevel.Pin        : m_screen = new Views.LoginPinView          { OnLogin = () => ResetLook(instance, callback, false) }; break;
            }
            Hide(instance);
        }

        Popup                m_popup;
        PhoneApplicationPage m_screen;
        Type                 m_viewModel;
        Action               m_callback;
        
        public void CheckPermissions()
        {
            if (m_screen == null)
            {
                m_callback.Invoke();
                return;
            }

            m_screen.Height =  App.Current.Host.Content.ActualHeight - 50;
            m_screen.Width  =  App.Current.Host.Content.ActualWidth;
            m_popup = new Popup
            {
                Child          = m_screen,
                VerticalOffset = 30,
                IsOpen         = true,
            };
        }

        void Hide(PhoneApplicationPage instance)
        {
            instance.Opacity = 0;
            instance.BackKeyPress += HidePopup;

            if (instance.ApplicationBar != null)
                instance.ApplicationBar.IsVisible = false;
        }

        void HidePopup(object s, System.ComponentModel.CancelEventArgs e)
        {
            var sender = (PhoneApplicationPage)s;
            m_popup.IsOpen = false;
            sender.BackKeyPress -= HidePopup;
        }

        void ResetLook(PhoneApplicationPage instance, Action callback, bool isLoggedIn = true)
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
