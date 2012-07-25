using System;
using System.ComponentModel;

namespace CMcG.CommonwealthBank.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected void NotifyPropertyChanged(params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public App CurrentApp
        {
            get { return App.Current; }
        }
    }
}
