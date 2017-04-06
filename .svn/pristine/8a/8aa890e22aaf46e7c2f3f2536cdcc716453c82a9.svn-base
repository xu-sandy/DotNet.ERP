using Pharos.POS.Retailing.ChildWin;
using Pharos.Wpf.HotKeyHelper;
using System;
using System.Windows;

namespace Pharos.POS.Retailing.Models.HotKeyCommands
{
    public class TuiHuoCommand : IHotKeyCommand
    {
        public Action<Window> Handler
        {
            get
            {
                return new Action<Window>((win) =>
                {
                    if (win is PosWindow)
                    {
                        TuiHuanHuo page = new TuiHuanHuo(1);
                        page.Owner = win;
                        page.ShowDialog();
                    }
                    else if (win is TuiHuanHuo) 
                    {
                        var page = (TuiHuanHuo)win;
                        page.SetTabItemShow(1);
                    }
                });
            }
        }
    }
}