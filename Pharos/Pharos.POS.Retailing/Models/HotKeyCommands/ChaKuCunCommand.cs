﻿using Pharos.POS.Retailing.ChildWin;
using Pharos.Wpf.HotKeyHelper;
using System;
using System.Windows;

namespace Pharos.POS.Retailing.Models.HotKeyCommands
{
    public class ChaKuCunCommand : IHotKeyCommand
    {
        public Action<Window> Handler
        {
            get
            {
                return new Action<Window>((win) =>
                {
                    ChaKuCun page = new ChaKuCun();
                    page.Owner = win;
                    page.ShowDialog();
                });
            }
        }
    }
}
