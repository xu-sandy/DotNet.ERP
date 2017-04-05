using Pharos.POS.Retailing.Models.PosModels;
using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.HotKeyHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Pharos.POS.Retailing.Models.HotKeyCommands
{
    public class ProductEditorCommand : IHotKeyCommand
    {
        public Action<Window> Handler
        {
            get
            {
                return new Action<Window>((win) =>
                {
                    var dg = win as IPosDataGrid;
                    if (dg != null && dg.CurrentGrid != null && dg.CurrentGrid.HasItems)
                    {
                        var items = (IEnumerable<IEdit>)dg.CurrentGrid.ItemsSource;
                        if (dg.CurrentGrid.SelectedIndex != -1)
                        {
                            items.ElementAt(dg.CurrentGrid.SelectedIndex).EditCommand.Execute(null);
                        }
                        else if (items.Count() > 0)
                        {
                            items.LastOrDefault().EditCommand.Execute(null);
                        }
                    }
                });
            }
        }
    }
}
