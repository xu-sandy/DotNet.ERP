using Pharos.POS.Retailing.ChildWin;
using Pharos.POS.Retailing.ChildWin.Pay;
using Pharos.POS.Retailing.Models.PosModels;
using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.HotKeyHelper;
using System;
using System.Linq;
using System.Windows;

namespace Pharos.POS.Retailing.Models.HotKeyCommands
{
    public class XianJinZhiFuCommand : IHotKeyCommand
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
                            var payItem = Global.Payways.FirstOrDefault(o => o.Mode == PayMode.CashPay);
                            if (payItem == null) return;
                            MultiPay page = new MultiPay(PosViewModel.Current.Receivable, PosModels.PayAction.Sale, payItem);
                            page.Owner = win;
                            page.ShowDialog();

                            //XianJinZhiFu page = new XianJinZhiFu(PosViewModel.Current.Receivable, PosModels.PayAction.Sale);
                            //page.Owner = win;
                            //page.ShowDialog();
                        }
                    }
                    //else if (win is ZhiFuFangShi)
                    //{
                    //    var window = win as ZhiFuFangShi;
                    //    XianJinZhiFu page = new XianJinZhiFu(window.Price, window.PayAction, window.Reason);
                    //    page.Owner = win.Owner;

                    //    win.Close();
                    //    page.ShowDialog();
                    //}
                    else if (win is ChoosePayWindow)
                    {
                        var window = win as ChoosePayWindow;
                        window.SetPayItem(PosModels.PayMode.CashPay);
                    }
                    else if (win is MultiPay)
                    {
                        var window = win as MultiPay;
                        window.SetPayItem(PosModels.PayMode.CashPay);
                    }
                });
            }
        }
    }
}
