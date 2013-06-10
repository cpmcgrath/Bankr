using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CMcG.Bankr.Data
{
    public class NotifyBase : INotifyPropertyChanging, INotifyPropertyChanged
    {
        static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(string.Empty);

        public event PropertyChangingEventHandler PropertyChanging = delegate { };
        public event PropertyChangedEventHandler  PropertyChanged  = delegate { };

        protected virtual void FirePropertyChanging()
        {
            PropertyChanging(this, emptyChangingEventArgs);
        }

        protected void FirePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void FirePropertyChanged(params Expression<Func<object>>[] properties)
        {
            foreach (var property in properties)
                FirePropertyChanged(GetMemberInfo(property).Name);
        }

        public static MemberInfo GetMemberInfo(Expression expression)
        {
            var lambda = (LambdaExpression)expression;

            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
                memberExpression = (MemberExpression)lambda.Body;

            return memberExpression.Member;
        }

        protected void SetValue<T>(ref T storedValue, T newValue, [CallerMemberName]string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(storedValue, newValue))
                return;

            FirePropertyChanging();
            storedValue = newValue;
            FirePropertyChanged(propertyName);
        }
    }
}
