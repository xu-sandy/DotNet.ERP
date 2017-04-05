﻿using Pharos.Wpf.Models;
using System;
using System.Linq;
using System.Windows.Data;

namespace Pharos.Wpf.XamlConverters
{
    public class EnumForTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var attributes = value.GetType().GetFields().Select(o=>o.GetCustomAttributes(typeof(EnumTitleAttribute), true));
            foreach (var item in attributes) 
            {
                foreach (EnumTitleAttribute attr in item) 
                {
                    if (attr.Value == (int)value) 
                    {
                        return attr.Title;
                    }
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var attributes = value.GetType().GetFields().Select(o => o.GetCustomAttributes(typeof(EnumTitleAttribute), true));
            foreach (var item in attributes)
            {
                foreach (EnumTitleAttribute attr in item)
                {
                    if (attr.Title == (string)value)
                    {
                        return attr.Value;
                    }
                }
            }
            return value;
        }
    }
}
