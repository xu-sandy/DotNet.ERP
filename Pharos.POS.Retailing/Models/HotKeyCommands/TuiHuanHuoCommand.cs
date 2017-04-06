using Pharos.POS.Retailing.ChildWin;
using Pharos.Wpf.HotKeyHelper;
using System;
using System.Windows;

namespace Pharos.POS.Retailing.Models.HotKeyCommands
{
    public class TuiHuanHuoCommand : IHotKeyCommand
    {
        public Action<Window> Handler
        {
            get
            {
                return new Action<Window>((win) =>
                {
                    TuiHuanHuo page = new TuiHuanHuo();
                    page.Owner = win;
                    page.ShowDialog();
                });
            }
        }
    }
}
