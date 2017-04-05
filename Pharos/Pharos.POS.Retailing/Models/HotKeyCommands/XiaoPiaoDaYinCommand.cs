﻿using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.HotKeyHelper;
using System;
using System.Windows;

namespace Pharos.POS.Retailing.Models.HotKeyCommands
{
    /// <summary>
    /// 小票打印快捷键
    /// </summary>
    public class XiaoPiaoDaYinCommand : IHotKeyCommand
    {
        public Action<Window> Handler
        {
            get
            {
                return new Action<Window>((win) =>
                {
                    switch (PosViewModel.Current.PrintStatus)
                    {
                        case PrintStatus.Close:
                            PosViewModel.Current.PrintStatus = PrintStatus.Open;
                            break;
                        case PrintStatus.Open:
                            PosViewModel.Current.PrintStatus = PrintStatus.Close;
                            break;
                    }
                });
            }
        }
    }
}
