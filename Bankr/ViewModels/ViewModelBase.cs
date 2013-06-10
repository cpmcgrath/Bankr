using Caliburn.Micro;
using CMcG.Bankr.Data;
using Microsoft.Phone.Controls;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls.Primitives;

namespace CMcG.Bankr.ViewModels
{
    public class ViewModelBase : ViewAware
    {
        public ViewModelBase() { }

        public ViewModelBase(INavigationService navigationService)
        {
            Navigator = navigationService;
            //FrameworkExtensions.CheckPermissions(GetType(), Navigator, OnLoad);
        }
        void Load()
        {
            OnLoad();
            Refresh();
        }

        protected virtual void OnLoad()
        {
        }

        protected void NotifyPropertyChanged(params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
                FirePropertyChanged(propertyName);
        }

        protected void FirePropertyChanged([CallerMemberName] string propertyName = "")
        {
            NotifyOfPropertyChange(propertyName);
        }

        

        public App CurrentApp
        {
            get { return App.Current; }
        }

        protected INavigationService Navigator { get; private set; }

        protected override void OnViewLoaded(object view)
        {
            if (Navigator != null)
                FrameworkExtensions.CheckPermissions(GetType(), Navigator, Load);
        }
    }
}
