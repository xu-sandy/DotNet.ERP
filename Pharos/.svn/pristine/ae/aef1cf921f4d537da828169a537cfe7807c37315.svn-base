using Pharos.POS.Retailing.Models.PosModels;
using Pharos.Wpf.HotKeyHelper;
using System;
using System.Windows;
using System.Windows.Input;

namespace Pharos.POS.Retailing.Models.HotKeyCommands
{
    public class SaoMaCommand : IHotKeyCommand
    {
        public Action<Window> Handler
        {
            get
            {
                return new Action<Window>((win) =>
                {
                    var control = win as IBarcodeControl;
                    Keyboard.Focus(control.CurrentIInputElement);
                });
            }
        }
    }
}

