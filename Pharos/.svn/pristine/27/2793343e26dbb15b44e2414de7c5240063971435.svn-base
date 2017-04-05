using System;
using System.Globalization;
using System.Windows.Data;

namespace Pharos.Barcode.Retailing.Converters
{
    public class TextPlaceholderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return string.IsNullOrEmpty(value.ToString());
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("IsNullConverter can only be used OneWay.");
        }
    }
}
