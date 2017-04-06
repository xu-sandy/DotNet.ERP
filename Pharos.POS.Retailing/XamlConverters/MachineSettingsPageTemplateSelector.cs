using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Pharos.POS.Retailing.XamlConverters
{
    public class MachineSettingsPageTemplateSelector : DataTemplateSelector
    {
        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            var obj = item as ISettingsItem;
            var parent = container as FrameworkElement;
            var template = parent.LoadResourceXaml(obj.XamlPath, UriKind.Relative);
            return template as DataTemplate;
        }
    }
}
