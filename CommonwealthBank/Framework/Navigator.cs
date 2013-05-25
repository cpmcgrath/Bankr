using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Navigation;

namespace CMcG.CommonwealthBank
{
    public class Navigator
    {
        NavigationService m_navigation;
        public Navigator(NavigationService navigation)
        {
            m_navigation = navigation;
        }

        public bool GoTo<TViewModel>(params object[] args)
        {
            var defaultScreenName = "Screen" + typeof(TViewModel).Name.Replace("ViewModel", "");
            var assembly = Assembly.GetExecutingAssembly();
            var type = assembly.GetTypes().FirstOrDefault(x => ViewOfAttribute.IsViewOf<TViewModel>(x))
                    ?? assembly.GetTypes().First(x => x.Name == defaultScreenName);

            var uri = type.FullName.Substring(type.FullName.IndexOf(".Views")).Replace('.', '/') + ".xaml";
            return m_navigation.Navigate(new Uri(uri + ProcessArgList<TViewModel>(args), UriKind.Relative));
        }

        string ProcessArgList<TViewModel>(object[] args)
        {
            if (args.Length == 0)
                return string.Empty;

            var parameters = typeof(TViewModel).GetConstructors()
                                               .Select(x => x.GetParameters())
                                               .First(y => y.Length == args.Length);

            var output = "?";
            for (int i = 0; i < args.Length; i++)
            {
                if (i != 0)
                    output += "&";
                output += parameters[i].Name + "=" + args[i].ToString();
            }

            return output;
        }

        public void GoBack(uint pagesToGoBack = 1)
        {
            for (uint i = 0; i < pagesToGoBack - 1; i++)
                m_navigation.RemoveBackEntry();

            m_navigation.GoBack();
        }

        public void Setup()
        {
        }
    }
}