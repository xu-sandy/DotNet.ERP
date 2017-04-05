using System.Windows;
using System.Windows.Controls;

namespace Pharos.Wpf.Controls
{
    public class OfficeTabControl : TabControl
    {
        static OfficeTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OfficeTabControl), new FrameworkPropertyMetadata(typeof(OfficeTabControl)));
        }
    }
}
