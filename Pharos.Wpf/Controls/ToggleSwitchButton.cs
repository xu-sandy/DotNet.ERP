using System.Windows;
using System.Windows.Controls;

namespace Pharos.Wpf.Controls
{
    public class ToggleSwitchButton : CheckBox
    {
        static ToggleSwitchButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleSwitchButton), new FrameworkPropertyMetadata(typeof(ToggleSwitchButton)));
        }
    }
}
