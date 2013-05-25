using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace CMcG.CommonwealthBank
{
    public static class NavigationExtensions
    {
        public static Navigator Navigation(this Page instance)
        {
            return new Navigator(instance.NavigationService);
        }
    }
}
