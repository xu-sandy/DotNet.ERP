using Pharos.POS.Retailing.ChildWin;
using Pharos.Wpf.HotKeyHelper;
using System;
using System.Windows;

namespace Pharos.POS.Retailing.Models.HotKeyCommands
{
    public class RiJieCommand : IHotKeyCommand
    {
        public Action<Window> Handler
        {
            get
            {
                return new Action<Window>((win) =>
                {
                    RiJie page = new RiJie();
                    page.Owner = win;
                    page.ShowDialog();
                });
            }
        }
    }
}
