using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Pharos.POS.Retailing.Models.PosModels;
using Pharos.Wpf.HotKeyHelper;

namespace Pharos.POS.Retailing.Models.HotKeyCommands
{
    //down
    public class DataGridNextItemCommand : IHotKeyCommand
    {
        public Action<Window> Handler
        {
            get
            {
                return new Action<Window>((win) =>
                {
                    var pagaDataGrid = win as IPosDataGrid;

                    if (pagaDataGrid != null)
                    {
                        if (pagaDataGrid.CurrentGrid != null)
                        {
                            var arr = pagaDataGrid.CurrentGrid.ItemsSource as IEnumerable<object>;

                            if (pagaDataGrid.CurrentGrid.SelectedIndex == -1 && arr != null && arr.Count() > 0)
                            {
                                pagaDataGrid.CurrentGrid.SelectedIndex = 0;
                                if (pagaDataGrid.CurrentGrid.SelectedItem != null)
                                {
                                    pagaDataGrid.CurrentGrid.ScrollIntoView(pagaDataGrid.CurrentGrid.SelectedItem);
                                }
                            }
                            else if (arr != null && arr.Count() > 0 && pagaDataGrid.CurrentGrid.SelectedIndex < arr.Count())
                            {
                                pagaDataGrid.CurrentGrid.SelectedIndex = pagaDataGrid.CurrentGrid.SelectedIndex + 1;
                                if (pagaDataGrid.CurrentGrid.SelectedItem != null)
                                {
                                    pagaDataGrid.CurrentGrid.ScrollIntoView(pagaDataGrid.CurrentGrid.SelectedItem);
                                }
                            }
                        }
                    }
                });
            }
        }
    }
}