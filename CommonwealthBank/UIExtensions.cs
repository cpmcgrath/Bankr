using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;

namespace CMcG.CommonwealthBank
{
    public static class UIExtensions
    {
        public static void UpdateBinding(this FrameworkElement instance, DependencyProperty prop)
        {
            var bindingExpression = instance.GetBindingExpression(prop);

            if (bindingExpression != null)
                bindingExpression.UpdateSource();
        }

        public static void FinishBinding(this PhoneApplicationPage instance)
        {
            var element = FocusManager.GetFocusedElement();

            if (element is TextBox)
                UpdateBinding((TextBox)element, TextBox.TextProperty);
            else if (element is PasswordBox)
                UpdateBinding((PasswordBox)element, PasswordBox.PasswordProperty);

            instance.Focus();
        }

        public static void CheckPermissions<TViewModel>(this PhoneApplicationPage instance, NavigationEventArgs e) where TViewModel : new()
        {
            instance.CheckPermissions(e, () => new TViewModel());
        }

        public static void CheckPermissions<TViewModel>(this PhoneApplicationPage instance, NavigationEventArgs e, Func<TViewModel> creator)
        {
            if (e.NavigationMode != NavigationMode.New)
                return;

            UIElement screen = null;

            switch (App.Current.Security.LogonRequired<TViewModel>())
            {
                case Security.LoginType.None        : instance.DataContext = creator.Invoke(); return;
                case Security.LoginType.CreateLogin : screen = new Views.Options.ScreenLoginEdit { OnSave  = () => ResetLook(instance, creator)        }; break;
                case Security.LoginType.Password    : screen = new Views.ScreenLogin             { OnLogin = () => ResetLook(instance, creator)        }; break;
                case Security.LoginType.Pin         : screen = new Views.ScreenLoginPin          { OnLogin = () => ResetLook(instance, creator, false) }; break;
            }

            Hide(instance);
            var popup = new Popup
            {
                Child          = screen,
                VerticalOffset = 30,
                IsOpen         = true,
            };
        }

        public static void Hide(PhoneApplicationPage instance)
        {
            instance.Opacity = 0;

            if (instance.ApplicationBar != null)
                instance.ApplicationBar.IsVisible = false;
        }

        static void ResetLook<TViewModel>(PhoneApplicationPage instance, Func<TViewModel> creator, bool isLoggedIn = true)
        {
            App.Current.Security.IsLoggedIn = true;
            instance.DataContext = creator.Invoke();
            instance.Opacity = 1;

            if (instance.ApplicationBar != null)
                instance.ApplicationBar.IsVisible = true;
        }
    }
}
