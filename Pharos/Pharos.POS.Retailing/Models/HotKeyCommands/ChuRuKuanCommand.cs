using Pharos.POS.Retailing.ChildWin;
using Pharos.Wpf.HotKeyHelper;
using System;
using System.Windows;

namespace Pharos.POS.Retailing.Models.HotKeyCommands
{
    public class ChuRuKuanCommand : IHotKeyCommand
    {
        public Action<Window> Handler
        {
            get
            {
                return new Action<Window>((win) =>
                {
                    ChuRuKuan page = new ChuRuKuan();
                    page.Owner = win;
                    page.ShowDialog();
                });
            }
        }
    }
}
