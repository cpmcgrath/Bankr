using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace CMcG.Bankr.Controls.Converters
{
    public class FloatToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            var amount = value is float ? (float)value
                       : value is decimal ? (float)(decimal)value
                       : 0;
            return amount == 0 ? new SolidColorBrush(Colors.Transparent)
                 : amount  > 0 ? new SolidColorBrush(Color.FromArgb(0xFF, 0x5B, 0xC9, 0x94))
                 :               new SolidColorBrush(Color.FromArgb(0xFF, 0xF6, 0x84, 0x84));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }
    }
}
