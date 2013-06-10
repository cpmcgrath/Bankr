﻿using Caliburn.Micro;
using System;
using System.Runtime.CompilerServices;

namespace CMcG.Bankr.ViewModels
{
    public class ViewModelBase : ViewAware
    {
        PermissionChecker m_checker;
        public ViewModelBase() { }

        public ViewModelBase(INavigationService navigationService)
        {
            Navigator = navigationService;
            m_checker = new PermissionChecker(GetType(), Navigator, Load);
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
            if (m_checker != null)
                m_checker.CheckPermissions();
        }
    }
}
