﻿using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.HotKeyHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Pharos.POS.Retailing.Models.HotKeyCommands
{
    public class ClearCommand : IHotKeyCommand
    {
        public Action<Window> Handler
        {
            get
            {
                return new Action<Window>((win) =>
                {
                    win.Dispatcher.Invoke(new Action(() =>
                    {
                        PosViewModel.Current.ClearOrder.Execute(null);
                    }));
                });
            }
        }
    }
}