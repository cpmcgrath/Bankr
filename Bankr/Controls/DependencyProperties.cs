using System;
using System.Linq;
using System.Windows;
using System.Linq.Expressions;

namespace CMcG.Bankr.Controls
{
    public static class DependencyProperties<TOwner> where TOwner : DependencyObject
    {
        #region Register(p => p.MyProp, default, (s, e) => s.OnMyPropChanged(e)
        public static DependencyProperty Register<TReturn>(Expression<Func<TOwner, TReturn>> property,
                                                           TReturn defaultValue = default(TReturn),
                                                           Action<TOwner, PropertyChangedEventArgs<TReturn>> changed = null)
        {
            var metadata = SetupMetadata(defaultValue, changed);
            return DependencyProperty.Register(GetName(property), typeof(TReturn), typeof(TOwner), metadata);
        }
        #endregion

        #region Register(p => p.MyProp, (s, e) => s.OnMyPropChanged(e))
        public static DependencyProperty Register<TReturn>(Expression<Func<TOwner, TReturn>>                 property,
                                                           Action<TOwner, PropertyChangedEventArgs<TReturn>> changed)
        {
            return Register(property, default(TReturn), changed);
        }
        #endregion

        #region Helpers
        static string GetName<TReturn>(Expression<Func<TOwner, TReturn>> property)
        {
            return GetMemberInfo(property).Name;
        }

        static System.Reflection.MemberInfo GetMemberInfo(LambdaExpression lambda)
        {
            System.Linq.Expressions.Expression result = null;

            switch (lambda.Body.NodeType)
            {
                case ExpressionType.Convert      : result = ((UnaryExpression)lambda.Body).Operand; break;
                case ExpressionType.MemberAccess : result = lambda.Body; break;
            }
          
            if (result is MemberExpression)
                return ((MemberExpression)result).Member;
          
            throw new ArgumentException("Not a member access", "member");
        }

        static PropertyMetadata SetupMetadata<TReturn>(TReturn defaultValue, Action<TOwner, PropertyChangedEventArgs<TReturn>> changed)
        {
            return changed == null
                 ? new PropertyMetadata(defaultValue)
                 : new PropertyMetadata(defaultValue, (s, e) => changed.Invoke((TOwner)s, new PropertyChangedEventArgs<TReturn>(e)));
        }
        #endregion
    }
}