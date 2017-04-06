using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.HotKeyHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Pharos.POS.Retailing.Models.HotKeyCommands
{
    class InputModeCommand : IHotKeyCommand
    {
        public Action<Window> Handler
        {
            get
            {
                return new Action<Window>((win) =>
                {
                    switch (HotKey.Mode)
                    {
                        case HotkeyMode.Open:
                            PosViewModel.Current.InputMode = HotkeyMode.Close;
                            break;
                        case HotkeyMode.Close:
                            PosViewModel.Current.InputMode = HotkeyMode.Open;
                            break;
                    }
                });
            }
        }
    }
}
