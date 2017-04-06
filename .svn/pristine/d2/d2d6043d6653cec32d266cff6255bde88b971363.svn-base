using Pharos.Wpf.HotKeyHelper;
using System;
using System.Windows;

namespace Pharos.POS.Retailing.Models.HotKeyCommands
{
    public class LockScreenCommand : IHotKeyCommand
    {
        public Action<Window> Handler
        {
            get
            {
                return new Action<Window>((o) =>
                {
                    LockScreen page = new LockScreen();
                    page.Show();
                    o.Hide();

                });
            }
        }
    }
}
