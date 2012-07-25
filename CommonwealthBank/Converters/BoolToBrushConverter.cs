using System;
using System.Windows.Data;
using System.Windows.Media;

namespace CMcG.CommonwealthBank.Converters
{
    public class BoolToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? TrueBrush : FalseBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public Brush TrueBrush  { get; set; }
        public Brush FalseBrush { get; set; }
    }
}
