using Pharos.POS.Retailing.Models;
using Pharos.POS.Retailing.Models.PosModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace Pharos.POS.Retailing.XamlConverters
{
    public class PayEnableConverter : MarkupExtension, IValueConverter
    {
        static PayEnableConverter converter;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var pay = Global.Payways.FirstOrDefault(o => o.Mode == (PayMode)parameter);
            if (pay != null)
            {
                return !pay.Enable;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (converter == null) converter = new PayEnableConverter();
            return converter;
        }
    }
}
