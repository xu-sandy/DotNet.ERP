using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Pharos.POS.Retailing.XamlConverters
{
    public class MultiPayDataTemplateSelector : DataTemplateSelector
    {
        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            if (item != null && item is IMultiPayViewModel)
            {
                IMultiPayViewModel multiPayViewModel = item as IMultiPayViewModel;

                if (multiPayViewModel.IsFrist)
                    return TitleTpl;
                else if (multiPayViewModel.IsLast)
                    return AddTpl;
                else
                    return ItemTpl;
            }
            return null;
        }

        public DataTemplate TitleTpl { get; set; }
        public DataTemplate ItemTpl { get; set; }
        public DataTemplate AddTpl { get; set; }
    }
    public interface IMultiPayViewModel
    {
        bool IsLast { get; set; }
        bool IsFrist { get; set; }
    }
}
