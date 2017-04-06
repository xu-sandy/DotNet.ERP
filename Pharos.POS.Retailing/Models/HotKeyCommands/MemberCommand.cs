using Pharos.POS.Retailing.ChildWin;
using Pharos.Wpf.HotKeyHelper;
using System;

namespace Pharos.POS.Retailing.Models.HotKeyCommands
{
    public class MemberCommand : IHotKeyCommand
    {
        public Action<System.Windows.Window> Handler
        {
            get
            {
                return new Action<System.Windows.Window>((o) =>
                {
                    Member page = new Member(1);
                    page.Owner = o;
                    page.ShowDialog();
                });
            }
        }
    }
}
