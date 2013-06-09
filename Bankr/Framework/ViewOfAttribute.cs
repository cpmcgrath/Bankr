using System;
using System.Linq;
using System.Reflection;

namespace CMcG.Bankr
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ViewOfAttribute : Attribute
    {
        public ViewOfAttribute(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }

        public Type ViewModelType { get; private set; }

        public static bool IsViewOf<TViewModel>(Type t)
        {
            var attr = t.GetCustomAttribute<ViewOfAttribute>();
            return attr != null && attr.ViewModelType == typeof(TViewModel);
        }
    }
}