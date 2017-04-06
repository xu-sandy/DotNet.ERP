using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.HotKeyHelper;
using System;
using System.Windows;

namespace Pharos.POS.Retailing.Models.HotKeyCommands
{
    /// <summary>
    /// 挂单快捷键
    /// </summary>
    public class GuaDanCommand : IHotKeyCommand
    {
        public Action<Window> Handler
        {
            get
            {
                return new Action<Window>((win) =>
                {
                    PosViewModel.Current.HandBill.Execute(null);
                });
            }
        }
    }
}
