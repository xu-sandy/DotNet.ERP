using System;
using System.IO;
using System.Windows;
using System.Windows.Markup;

namespace Pharos.Wpf.Extensions
{
    public static class FrameworkElementExtensions
    {
        public static object LoadResourceXaml(string url, UriKind mode)
        {
            Uri uri = new Uri(url, mode);
            Stream stream = Application.GetResourceStream(uri).Stream;
            //FrameworkElement继承自UIElement
            return XamlReader.Load(stream);
        }

        public static object LoadResourceXaml(this FrameworkElement _this, string url, UriKind mode)
        {
            return LoadResourceXaml(url, mode);
        }
    }
}
