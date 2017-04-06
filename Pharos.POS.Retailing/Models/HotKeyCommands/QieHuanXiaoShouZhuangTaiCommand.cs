using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.HotKeyHelper;
using System;
using System.Windows;

namespace Pharos.POS.Retailing.Models.HotKeyCommands
{
    public class QieHuanXiaoShouZhuangTaiCommand : IHotKeyCommand
    {
        public Action<Window> Handler
        {
            get
            {
                return new Action<Window>((win) =>
                {
                    switch (PosViewModel.Current.PosStatus)
                    {
                        case PosStatus.Gift:
                            PosViewModel.Current.PosStatus = PosStatus.Normal;
                            break;
                        case PosStatus.Normal:
                            PosViewModel.Current.PosStatus = PosStatus.Gift;
                            break;
                    }
                });
            }
        }
    }
}
