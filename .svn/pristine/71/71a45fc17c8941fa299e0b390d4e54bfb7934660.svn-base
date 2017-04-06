using Pharos.POS.Retailing.ChildWin;
using Pharos.Wpf.HotKeyHelper;
using System;

namespace Pharos.POS.Retailing.Models.HotKeyCommands
{
    public class XuanZheHuoDongCommand : IHotKeyCommand
    {
        public Action<System.Windows.Window> Handler
        {
            get
            {
                return new Action<System.Windows.Window>((o) =>
                {
                    HuoDongXuanZhe page = new HuoDongXuanZhe();
                    page.Owner = o;
                    page.ShowDialog();
                });
            }
        }
    }
}
