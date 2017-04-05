﻿using Pharos.POS.Retailing.ChildWin;
using Pharos.POS.Retailing.ChildWin.Pay;
using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.HotKeyHelper;
using System;
using System.Windows;

namespace Pharos.POS.Retailing.Models.HotKeyCommands
{
    /// <summary>
    /// 多方式支付快捷键
    /// </summary>
    public class DuoFangShiZhiFuCommand : IHotKeyCommand
    {
        public Action<Window> Handler
        {
            get
            {
                return new Action<Window>((win) =>
                {
                    if (win is PosWindow)
                    {
                        if (PosViewModel.Current.OrderList.Count > 0)
                        {
                            MultiPay page = new MultiPay(PosViewModel.Current.Receivable, PosModels.PayAction.Sale);
                            page.Owner = win;
                            page.ShowDialog();
                        }
                    }
                    //else if (win is ZhiFuFangShi)
                    //{
                    //    var window = win as ZhiFuFangShi;
                    //    MultiPay page = new MultiPay(window.Price, window.PayAction);
                    //    page.Owner = win.Owner;

                    //    win.Close();
                    //    page.ShowDialog();
                    //}
                });
            }
        }
    }
}
