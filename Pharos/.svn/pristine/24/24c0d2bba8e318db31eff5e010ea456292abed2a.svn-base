using Pharos.POS.Retailing.Models.PosModels;
using Pharos.POS.Retailing.Models.Printer;
using Pharos.Wpf.ViewModelHelpers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using Pharos.POS.Retailing.Models.ApiReturnResults;
using System.Windows;
using Pharos.POS.Retailing.Models.ApiParams;
using System.Collections.ObjectModel;
using Pharos.POS.Retailing.Models.ViewModels.Pay;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class PayComplete : BaseViewModel, IDisposable
    {
        private DateTime OrderTime { get; set; }

        private bool isRunning = true;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_amount">应收</param>
        /// <param name="Receipt">实收</param>
        /// <param name="change">找零</param>
        public PayComplete(decimal _amount, decimal receipt, decimal change, IEnumerable<MultiPayItemViewModel> payItems, PayAction payAction, DateTime orderTime)
        {
            PosViewModel.Current.IsClearStatus = true;

            OrderTime = orderTime;
            Amount = _amount;
            ViewModelBinded += PayComplete_ViewModelBinded;
            //print
            #region 退换货打印
            if ((payAction == PayAction.Change || payAction == PayAction.Refund || payAction == PayAction.RefundAll) && PosViewModel.Current.PrintStatus == PrintStatus.Open)
            {
                Task.Factory.StartNew(() =>
                {
                    //Printer
                    var _machineInfo = Global.MachineSettings.MachineInformations;
                    int printWidth = 40;
                    PrintModelHelper printer = new PrintModelHelper();
                    TicketModel ticketModel = new TicketModel();
                    ticketModel.TicketWidth = printWidth;//发票宽度，按字符数计算，根据打印机型号有所区别(通常在30-70之间),建议系统提供配置入口
                    ticketModel.StoreName = _machineInfo.StoreName;
                    ticketModel.DeviceNumber = _machineInfo.MachineSn;
                    ticketModel.Cashier = Global.CurrentSaleMen.UserCode;
                    List<ProductModel> productList = new List<ProductModel>();

                    IEnumerable<ChangingList> changeList = null;
                    switch (payAction)
                    {
                        case PayAction.Change:
                            changeList = RefundChangeViewModel.Current.Change.ChangeList;
                            ticketModel.SN = RefundChangeViewModel.Current.Change.PaySn;
                            break;
                        case PayAction.Refund:
                            changeList = RefundChangeViewModel.Current.Refund.ChangeList;
                            ticketModel.SN = RefundChangeViewModel.Current.Refund.PaySn;
                            break;
                        case PayAction.RefundAll:
                            ticketModel.SN = RefundChangeViewModel.Current.RefundOrder.CurrentReturnOrderSn;
                            foreach (var item in RefundChangeViewModel.Current.RefundOrder.OrderList)
                            {
                                ProductModel productModel = new ProductModel();
                                productModel.Code = item.Barcode;
                                productModel.Name = item.Title;
                                productModel.Num = item.Number;
                                productModel.Price = item.ActualPrice;
                                productModel.SubTotal = item.Total;
                                productList.Add(productModel);
                            }
                            ticketModel.CountNum = (int)RefundChangeViewModel.Current.RefundOrder.OrderList.Sum(o => o.Number);//对应状态
                            ticketModel.OrderType = 3;//对应状态  3=退单

                            break;
                    }

                    if (payAction == PayAction.Change || payAction == PayAction.Refund)
                    {
                        foreach (var item in changeList)
                        {
                            ProductModel productModel = new ProductModel();
                            productModel.Code = item.Barcode;
                            productModel.Name = item.Title;
                            productModel.Num = item.ChangeNumber;
                            productModel.Price = item.ChangePrice;
                            productModel.SubTotal = item.Total;
                            productList.Add(productModel);
                        }
                        ticketModel.CountNum = (int)changeList.Sum(o => o.ChangeNumber);//对应状态
                        if (payAction == PayAction.Change)
                            ticketModel.OrderType = (int)RefundChangeViewModel.Current.Change.Status;//对应状态
                        if (payAction == PayAction.Refund)
                            ticketModel.OrderType = (int)RefundChangeViewModel.Current.Refund.Status;//对应状态
                    }
                    ticketModel.ProductList = productList;
                    ticketModel.TotalPrice = _amount.ToString("0.###");
                    ticketModel.Receivable = receipt.ToString("0.###");
                    ticketModel.Change = change;
                    ticketModel.CreateDT = orderTime;

                    //  ticketModel.Weigh = "0 KG";
                    ticketModel.PayType = (payItems.Select(o => o.PayItem).Distinct().Count() == 1 ? payItems.FirstOrDefault(o => o.PayItem != null).PayName : "多方付");

                    //把会员卡对应的卡号 卡余额 加到打印对象
                    List<Dictionary<string, string>> cardAndBalance = new List<Dictionary<string, string>>();
                    foreach (var item in payItems)
                    {
                        if (item.PayItem.Mode == PayMode.StoredValueCard)
                        {
                            Dictionary<string, string> dic = new Dictionary<string, string>();
                            dic.Add(item.CardNo, item.Balance.ToString());
                            cardAndBalance.Add(dic);
                        }
                    }
                    ticketModel.CardAndBalances = cardAndBalance;

                    List<string> footItemList = new List<string>();
                    //if (PosViewModel.Current.Preferential > 0m)
                    //{
                    //    footItemList.Add("已优惠：" + string.Format("{0:N2}", 0));

                    //}

                    //footItemList.Add("称重商品数量请参照条码标签");
                    footItemList.Add("欢迎光临");
                    footItemList.Add("服务电话：" + _machineInfo.Phone);
                    footItemList.Add("请保留电脑小票，作为退换货凭证");
                    footItemList.Add("退换小票");
                    ticketModel.FootItemList = footItemList;
                    string titleStr = string.Empty; string printStr = printer.GetPrintStr(ticketModel, out titleStr);
                    PrintHelper.Print(printStr, titleStr);

                });
            }
            #endregion


            if (payAction == PayAction.Sale && PosViewModel.Current.PrintStatus == PrintStatus.Open)
            {
                Task.Factory.StartNew(() =>
                {
                    //Printer
                    var _machineInfo = Global.MachineSettings.MachineInformations;
                    int printWidth = 40;
                    PrintModelHelper printer = new PrintModelHelper();
                    TicketModel ticketModel = new TicketModel();
                    ticketModel.TicketWidth = printWidth;//发票宽度，按字符数计算，根据打印机型号有所区别(通常在30-70之间),建议系统提供配置入口
                    ticketModel.StoreName = _machineInfo.StoreName;
                    ticketModel.DeviceNumber = _machineInfo.MachineSn;
                    ticketModel.SN = PosViewModel.Current.OrderSn;
                    ticketModel.Cashier = Global.CurrentSaleMen.UserCode;
                    ticketModel.Preferential = PosViewModel.Current.Preferential;

                    ticketModel.SaleMan = PosViewModel.Current.SalemanName;


                    List<ProductModel> productList = new List<ProductModel>();
                    foreach (var item in PosViewModel.Current.OrderList)
                    {
                        ProductModel productModel = new ProductModel();
                        productModel.Code = item.Barcode;
                        productModel.Name = item.Title;
                        productModel.Num = item.Number;
                        productModel.Price = item.ActualPrice;
                        productModel.SubTotal = item.Total;
                        productList.Add(productModel);
                    }
                    ticketModel.ProductList = productList;
                    ticketModel.CountNum = (int)PosViewModel.Current.Num;
                    ticketModel.TotalPrice = _amount.ToString("0.###");
                    ticketModel.Receivable = receipt.ToString("0.###");
                    ticketModel.Change = change;
                    ticketModel.OrderType = 0;
                    ticketModel.CreateDT = orderTime;
                    //  ticketModel.Weigh = "0 KG";
                    ticketModel.PayType = (payItems.Select(o => o.PayItem).Distinct().Count() == 1 ? payItems.FirstOrDefault(o => o.PayItem != null).PayName : "多方付");

                    //把会员卡对应的卡号 卡余额 加到打印对象


                    List<Dictionary<string, string>> cardAndBalance = new List<Dictionary<string, string>>();
                    foreach (var item in payItems)
                    {
                        if (item.PayItem.Mode == PayMode.StoredValueCard)
                        {
                            Dictionary<string, string> dic = new Dictionary<string, string>();
                            dic.Add(item.CardNo, item.Balance.ToString());
                            cardAndBalance.Add(dic);
                        }
                    }
                    ticketModel.CardAndBalances = cardAndBalance;


                    List<string> footItemList = new List<string>();
                    //if (PosViewModel.Current.Preferential > 0m)
                    //{
                    //    footItemList.Add("已优惠：" + string.Format("{0:N2}", PosViewModel.Current.Preferential));

                    //}

                    //footItemList.Add("称重商品数量请参照条码标签");
                    footItemList.Add("欢迎光临");
                    footItemList.Add("服务电话：" + _machineInfo.Phone);
                    footItemList.Add("请保留电脑小票，作为退换货凭证");
                    ticketModel.FootItemList = footItemList;
                    string titleStr = string.Empty; string printStr = printer.GetPrintStr(ticketModel, out titleStr);
                    PrintHelper.Print(printStr, titleStr);
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        PosViewModel.Current.ClearOrder.Execute(null);
                    }));

                });
            }
            else
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    PosViewModel.Current.ClearOrder.Execute(null);
                }));
            }


        }

        void PayComplete_ViewModelBinded(BaseViewModel model)
        {
            Task.Factory.StartNew(() =>
            {
                while (isRunning)
                {
                    CurrentWindow.Dispatcher.Invoke(new Action(() =>
                    {
                        if (Second == 0)
                        {
                            Dispose();
                            CurrentWindow.Close();
                            //PosViewModel.Current.ClearOrder.Execute(null);
                            return;
                        }
                        else
                        {
                            Second--;
                        }
                    }));
                    Thread.Sleep(1000);
                }
            });
        }



        private int _Second = 3;

        public int Second
        {
            get { return _Second; }
            set
            {
                _Second = value;
                this.OnPropertyChanged(o => o.Second);
            }
        }

        /// <summary>
        /// 成功支付金额
        /// </summary>
        private decimal amount;

        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        /// <summary>
        /// 关闭当前窗口
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    CurrentWindow.Close();
                    //  PosViewModel.Current.ClearOrder.Execute(null);
                });
            }
        }


        public void Dispose()
        {
            isRunning = false;
        }
        ~PayComplete()
        {
            Dispose();
        }
    }
}
