using System;
using System.Net;
using System.ComponentModel;

namespace CMcG.CommonwealthBank.Data
{
    public class NotifyBase : INotifyPropertyChanging, INotifyPropertyChanged
    {
        static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        public event PropertyChangingEventHandler PropertyChanging = delegate { };
        public event PropertyChangedEventHandler  PropertyChanged  = delegate { };

        protected virtual void SendPropertyChanging()
        {
            PropertyChanging(this, emptyChangingEventArgs);
        }

        protected virtual void SendPropertyChanged(params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
