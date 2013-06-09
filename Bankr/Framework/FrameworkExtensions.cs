using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using System.Windows;
using System.Windows.Controls.Primitives;
using CMcG.CommonwealthBank.Data;

namespace CMcG.CommonwealthBank
{
    public static class FrameworkExtensions
    {
        public static Navigator Navigation(this Page instance)
        {
            return new Navigator(instance.NavigationService);
        }

        public static void SetupView<TPage>(this TPage instance, NavigationEventArgs e) where TPage : PhoneApplicationPage
        {
            var defaultVMName = typeof(TPage).Name + "Model";
            var attr          = typeof(TPage).GetCustomAttribute<ViewOfAttribute>();
            var vm            = attr != null ? attr.ViewModelType : Navigator.GetViewModelTypes(false, true).First(x => x.Name == defaultVMName);

            var argLookup = instance.NavigationContext.QueryString;
            var constructor = vm.GetConstructors()
                                .Select(x => x.GetParameters())
                                .First(y => y.Length == argLookup.Count);

            CheckPermissions(instance, vm, e, () => Activator.CreateInstance(vm, GetArgs(vm, instance.NavigationContext.QueryString)));
        }

        static object[] GetArgs(Type toConstruct, IDictionary<string, string> argLookup)
        {
            var parameters = toConstruct.GetConstructors()
                                        .Select(x => x.GetParameters())
                                        .First(y => y.Length == argLookup.Count);

            return parameters.Select(x => Convert(x.ParameterType, argLookup[x.Name])).ToArray();
        }

        static object Convert(Type type, string value)
        {
            if (type == typeof(int))
                return int.Parse(value);

            if (type == typeof(decimal))
                return decimal.Parse(value);

            return value;
        }

        static Popup s_popup;

        static void CheckPermissions(PhoneApplicationPage instance, Type viewModel, NavigationEventArgs e, Func<object> creator)
        {
            if (s_popup != null && s_popup.IsOpen)
                s_popup.IsOpen = false;

            if (e.NavigationMode != NavigationMode.New)
                return;

            PhoneApplicationPage screen = null;

            switch (App.Current.Security.LogonRequired(viewModel))
            {
                case AccessLevel.None        : instance.DataContext = creator.Invoke(); return;
                case AccessLevel.CreateLogin : screen = new Views.Options.LoginEditView { OnSave  = () => ResetLook(instance, creator)        }; break;
                case AccessLevel.Password    : screen = new Views.LoginView             { OnLogin = () => ResetLook(instance, creator)        }; break;
                case AccessLevel.Pin         : screen = new Views.LoginPinView          { OnLogin = () => ResetLook(instance, creator, false) }; break;
            }

            Hide(instance);
            screen.Height =  Application.Current.Host.Content.ActualHeight - 50;
            screen.Width  =  Application.Current.Host.Content.ActualWidth;
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
