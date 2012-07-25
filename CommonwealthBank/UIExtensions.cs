using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Input;
using System.Windows.Controls.Primitives;

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

        public static void CheckPermissions<TViewModel>(this PhoneApplicationPage instance) where TViewModel : new()
        {
            instance.CheckPermissions(() => new TViewModel());
        }

        public static void CheckPermissions<TViewModel>(this PhoneApplicationPage instance, Func<TViewModel> creator)
        {
            if (instance.DataContext is TViewModel)
                return;

            if (App.Current.Security.HasPermission<TViewModel>())
            {
                instance.DataContext = creator.Invoke();
                return;
            }
            Hide(instance);

            var screen = App.Current.Security.HasLogin
                       ? new Views.ScreenLogin     { OnLogin = () => ResetLook(instance, creator) } as UIElement
                       : new Views.ScreenLoginEdit { OnSave  = () => ResetLook(instance, creator) };

            var popup = new Popup
            {
                Child          = screen,
                VerticalOffset = 30,
                IsOpen         = true
            };
        }

        public static void Hide(PhoneApplicationPage instance)
        {
            instance.Opacity = 0;

            if (instance.ApplicationBar != null)
                instance.ApplicationBar.IsVisible = false;
        }

        static void ResetLook<TViewModel>(PhoneApplicationPage instance, Func<TViewModel> creator)
        {
            App.Current.Security.IsLoggedIn = true;
            instance.DataContext = creator.Invoke();
            instance.Opacity = 1;

            if (instance.ApplicationBar != null)
                instance.ApplicationBar.IsVisible = true;
        }
    }
}
