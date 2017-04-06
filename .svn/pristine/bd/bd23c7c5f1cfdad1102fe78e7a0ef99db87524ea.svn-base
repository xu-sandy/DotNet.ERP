using Pharos.POS.Retailing.Models.ViewModels;
using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace Pharos.POS.Retailing.XamlConverters
{
    public class RowToIndexConverter : MarkupExtension, IValueConverter
    {
        static RowToIndexConverter converter;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return PosViewModel.Current.OrderList.IndexOf(value as Product) + 1;
            //DataGridRow row = value as DataGridRow;

            //if (row != null)
            //    return row.GetIndex() + 1;
            //else
            //    return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (converter == null) converter = new RowToIndexConverter();
            return converter;
        }
    }
}
