using Caliburn.Micro;
using CMcG.Bankr.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CMcG.Bankr
{
    public class Bootstrap : PhoneBootstrapper
    {
        PhoneContainer container;
 
        protected override void Configure()
        {
            container = new PhoneContainer(RootFrame);
 
            container.RegisterPhoneServices();
            foreach (var vm in App.GetViewModelTypes())
                container.RegisterPerRequest(vm, null, vm);
 
            AddCustomConventions();
        }
 
        static void AddCustomConventions()
        {
            ViewLocator.DeterminePackUriFromType = (viewModelType, viewType) =>
            {
                var baseNamespace  = typeof(App).Namespace;
                var uri = viewType.FullName.Replace(baseNamespace, string.Empty).Replace(".", "/") + ".xaml";
 
                if (!typeof(App).Assembly.GetAssemblyName().Equals(viewType.Assembly.GetAssemblyName()))
                {
                    return "/" + baseNamespace + ";component" + uri;
                }
 
                return uri;
            };
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

 
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }
 
        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

    }
}
