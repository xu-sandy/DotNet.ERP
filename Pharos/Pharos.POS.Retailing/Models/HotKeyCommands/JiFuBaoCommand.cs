using Pharos.POS.Retailing.ChildWin;
using Pharos.POS.Retailing.ChildWin.Pay;
using Pharos.POS.Retailing.Models.PosModels;
using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.HotKeyHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Pharos.POS.Retailing.Models.HotKeyCommands
{
    public class JiFuBaoCommand : IHotKeyCommand
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
                            var payItem = Global.Payways.FirstOrDefault(o => o.Mode == PayMode.JiFuBao);
                            if (payItem == null) return;
                            MultiPay page = new MultiPay(PosViewModel.Current.Receivable, PosModels.PayAction.Sale, payItem);
                            page.Owner = win;
                            page.ShowDialog();
                            //JiFuBao page = new JiFuBao(PosViewModel.Current.Receivable, PosModels.PayAction.Sale);
                            //page.Owner = win;
                            //page.ShowDialog();
                        }
                    }
                    //else if (win is ZhiFuFangShi)
                    //{
                    //    var window = win as ZhiFuFangShi;
                    //    JiFuBao page = new JiFuBao(window.Price, window.PayAction, window.Reason);
                    //    page.Owner = win.Owner;

                    //    win.Close();
                    //    page.ShowDialog();
                    //}
                    else if (win is ChoosePayWindow)
                    {
                        var window = win as ChoosePayWindow;
                        window.SetPayItem(PosModels.PayMode.JiFuBao);
                    }
                    else if (win is MultiPay)
                    {
                        var window = win as MultiPay;
                        window.SetPayItem(PosModels.PayMode.JiFuBao);
                    }
                });
            }
        }
    }
}
