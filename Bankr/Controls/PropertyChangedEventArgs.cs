using System;
using System.Linq;
using System.Windows;

namespace CMcG.Bankr.Controls
{
    public class PropertyChangedEventArgs<T>
    {
        public DependencyPropertyChangedEventArgs Source { get; private set; }

        public PropertyChangedEventArgs(DependencyPropertyChangedEventArgs source)
        {
            Source = source;
        }

        public T NewValue
        {
            get { return (T)Source.NewValue; }
        }

        public T OldValue
        {
            get { return (T)Source.OldValue; }
        }

        public DependencyProperty Property
        {
            get { return Source.Property; }
        }
    }
}