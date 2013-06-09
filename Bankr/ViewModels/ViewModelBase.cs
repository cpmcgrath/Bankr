using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CMcG.Bankr.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected void NotifyPropertyChanged(params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void FirePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public App CurrentApp
        {
            get { return App.Current; }
        }
    }
}
