using System;
using System.Linq;
using System.Windows;
using System.Linq.Expressions;

namespace CMcG.CommonwealthBank.Controls
{
    public static class DependencyProperties<TOwner> where TOwner : DependencyObject
    {
        public static DependencyProperty Register<TReturn>(Expression<Func<TOwner, TReturn>> property,
                                                           TReturn defaultValue = default(TReturn),
                                                           Action<TOwner, PropertyChangedEventArgs<TReturn>> changed = null)
        {
            var metadata = SetupMetadata(defaultValue, changed);
            return DependencyProperty.Register(GetName(property), typeof(TReturn), typeof(TOwner), metadata);
        }

        static string GetName<TReturn>(Expression<Func<TOwner, TReturn>> property)
        {
            return GetMemberInfo(property).Name;
        }

        private static System.Reflection.MemberInfo GetMemberInfo(System.Linq.Expressions.Expression member)
        {
            var lambda = (LambdaExpression)member;
            MemberExpression memberExpr = null;

            switch (lambda.Body.NodeType)
            {
                case ExpressionType.Convert      : memberExpr = ((UnaryExpression)lambda.Body).Operand as MemberExpression; break;
                case ExpressionType.MemberAccess : memberExpr = lambda.Body as MemberExpression; break;
            }
          
            if (memberExpr == null)
                throw new ArgumentException("Not a member access", "member");
          
            return memberExpr.Member;
        }

        #region Register<int>(p => p.MyProp, (s, e) => s.OnMyPropChanged(e))
        public static DependencyProperty Register<TReturn>(Expression<Func<TOwner, TReturn>>                 property,
                                                           Action<TOwner, PropertyChangedEventArgs<TReturn>> changed)
        {
            return Register(property, default(TReturn), changed);
        }
        #endregion

        

        static PropertyMetadata SetupMetadata<TReturn>(TReturn defaultValue, Action<TOwner, PropertyChangedEventArgs<TReturn>> changed)
        {
            return changed == null
                 ? new PropertyMetadata(defaultValue)
                 : new PropertyMetadata(defaultValue, (s, e) => changed.Invoke((TOwner)s, new PropertyChangedEventArgs<TReturn>(e)));
        }
    }
}