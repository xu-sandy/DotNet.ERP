using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.HotKeyHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Pharos.POS.Retailing.Models.HotKeyCommands
{
    public class GouMaiZuiHouYiDanCommand : IHotKeyCommand
    {
        public Action<Window> Handler
        {
            get
            {
                return new Action<Window>((o) =>
                {
                    if (PosViewModel.Current.OrderList.Count > 0)
                    {
                       var product = PosViewModel.Current.OrderList.LastOrDefault();
                       PosViewModel.Current.Barcode = product.Barcode;
                    }
                });
            }
        }
    }
}
