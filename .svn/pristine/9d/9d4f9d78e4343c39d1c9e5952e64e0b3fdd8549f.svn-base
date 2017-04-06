using Pharos.POS.Retailing.ChildWin;
using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.HotKeyHelper;
using System;
using System.Windows;
using System.Linq;
using System.Collections.Generic;
using Pharos.POS.Retailing.Models.PosModels;

namespace Pharos.POS.Retailing.Models.HotKeyCommands
{
    public class GoToCommand : IHotKeyCommand
    {
        public Action<Window> Handler
        {
            get
            {
                return new Action<Window>((o) =>
                {
                    var pagaDataGrid = o as IPosDataGrid;

                    if (pagaDataGrid != null)
                    {
                        var arr = pagaDataGrid.CurrentGrid.ItemsSource as IEnumerable<object>;
                        if (arr != null && arr.Count() > 0)
                        {
                            Goto page = new Goto();
                            page.Owner = o;
                            page.ShowDialog();
                        }
                    }
                });
            }
        }
    }
}
