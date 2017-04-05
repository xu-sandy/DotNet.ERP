﻿using Pharos.POS.Retailing.Models.ApiParams;
using Pharos.POS.Retailing.Models.ApiReturnResults;
using Pharos.Wpf.ViewModelHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Pharos.POS.Retailing.Models.Printer;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class StoredValueCardRecharge : BaseViewModel
    {

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                this.OnPropertyChanged(o => o.Name);
            }
        }


        private string cardNo;
        public string CardNo
        {
            get { return cardNo; }
            set
            {
                var text = value.Trim();

                if (text.Length > 40)
                {
                    Toast.ShowMessage("卡号超出！限制长度50个字符！", CurrentWindow);
                    return;
                }
                if (cardNo != text)
                {
                    Amount = 0m;
                    Name = "";
                    YuE = 0m;
                    CardState = "";
                }
                cardNo = text;
                this.OnPropertyChanged(o => o.CardNo);
            }
        }
        private string cardState;
        public string CardState
        {
            get { return cardState; }
            set
            {
                cardState = value;
                this.OnPropertyChanged(o => o.CardState);
            }
        }

        decimal amount;
        public decimal Amount
        {
            get { return amount; }
            set
            {

                amount = decimal.Truncate(value);
                this.OnPropertyChanged(o => o.Amount);
            }
        }
        decimal _YuE;
        public decimal YuE
        {
            get { return _YuE; }
            set
            {

                _YuE = value;
                this.OnPropertyChanged(o => o.YuE);
            }
        }
        public ICommand QueryCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    if (string.IsNullOrEmpty(CardNo))
                    {
                        Toast.ShowMessage("请输入卡号！", CurrentWindow);
                        return;
                    }
                    Task.Factory.StartNew(() =>
                    {
                        var _machinesInfo = Global.MachineSettings.MachineInformations;
                        StoredValueCardRechargeParams _params = new StoredValueCardRechargeParams()
                        {
                            StoreId = _machinesInfo.StoreId,
                            MachineSn = _machinesInfo.MachineSn,
                            CID = _machinesInfo.CompanyId,
                            CardNo = CardNo,
                            Amount = 0
                        };
                        var result = ApiManager.Post<StoredValueCardRechargeParams, ApiRetrunResult<StoredValueCardRechargeResult>>(@"api/StoredValueCardRecharge", _params);
                        CurrentWindow.Dispatcher.Invoke(new Action(() =>
                        {
                            if (result.Code == "200")
                            {
                                YuE = result.Result.balance;

                                Name = result.Result.Name;
                                CardNo = result.Result.CardNo;
                                CardState = result.Result.StateInfo;
                                if (o1 != null)
                                {
                                    if (o1.Equals("2"))
                                    {
                                        YuE = YuE - Amount;
                                    }
                                    else if (o1.Equals("1"))
                                    {
                                        YuE = YuE + Amount;
                                    }
                                }
                                else
                                {
                                    Amount = result.Result.Amount;
                                }
                            }
                            else
                            {
                                Toast.ShowMessage(result.Message, CurrentWindow);
                            }
                        }));
                    });
                });
            }
        }
        public ICommand ConfirmCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    if (string.IsNullOrEmpty(CardNo))
                    {
                        Toast.ShowMessage("请输入卡号！", CurrentWindow);
                        return;
                    }
                    QueryCommand.Execute(o1);

                    if (Amount <= 0)
                    {
                        Toast.ShowMessage((o1.Equals("1") ? "充值" : "反结算") + "金额不能为0！", CurrentWindow);
                        return;
                    }
                    Confirm.ShowMessage(string.Format("请确认是否{1} {0:N2} 元！", Amount, o1.Equals("1") ? "充值" : "反结算"), CurrentWindow, (state) =>
                    {
                        if (state == ConfirmMode.Cancelled)
                        {
                            QueryCommand.Execute(null);
                            return;
                        }

                        Task.Factory.StartNew(() =>
                        {
                            var _machinesInfo = Global.MachineSettings.MachineInformations;
                            StoredValueCardRechargeParams _params = new StoredValueCardRechargeParams()
                            {

                                StoreId = _machinesInfo.StoreId,
                                MachineSn = _machinesInfo.MachineSn,
                                CID = _machinesInfo.CompanyId,
                                CardNo = CardNo,
                                Amount = Amount,
                                Type = Convert.ToInt32(o1)
                            };
                            var result = ApiManager.Post<StoredValueCardRechargeParams, ApiRetrunResult<StoredValueCardRechargeResult>>(@"api/StoredValueCardRecharge", _params);
                            CurrentWindow.Dispatcher.Invoke(new Action(() =>
                            {
                                if (result.Code == "200")
                                {
                                    YuE = result.Result.balance;
                                    Amount = result.Result.Amount;
                                    Name = result.Result.Name;
                                    CardState = result.Result.StateInfo;
                                    CardNo = result.Result.CardNo;
                                    if (o1.Equals("1"))
                                    {
                                        //打印充值小票
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

                                            RechargeModel model = new RechargeModel();

                                            model.BeforeAmount = YuE - Amount;
                                            model.RechargeAmount = Amount;
                                            model.CurrentBalance = YuE;
                                            ticketModel.rechargeModel = model;

                                            ticketModel.PayType = "现金";
                                            ticketModel.Receivable = Amount.ToString("f2");
                                            ticketModel.SN = CardNo;
                                            //List<ProductModel> productList = new List<ProductModel>();

                                            //IEnumerable<ChangingList> changeList = null;
                                            ////ticketModel.ProductList = productList;
                                            ////ticketModel.TotalPrice = _amount.ToString("0.###");
                                            ////ticketModel.Receivable = receipt.ToString("0.###");
                                            ////ticketModel.Change = change;
                                            ////ticketModel.CreateDT = orderTime;

                                            //////  ticketModel.Weigh = "0 KG";
                                            ////ticketModel.PayType = (payItems.Select(o => o.PayItem).Distinct().Count() == 1 ? payItems.FirstOrDefault(o => o.PayItem != null).PayName : "多方付");


                                            List<string> footItemList = new List<string>();
                                            //if (PosViewModel.Current.Preferential > 0m)
                                            //{
                                            //    footItemList.Add("已优惠：" + string.Format("{0:N2}", 0));

                                            //}

                                            //footItemList.Add("称重商品数量请参照条码标签");
                                            footItemList.Add("欢迎光临");
                                            footItemList.Add("服务电话：" + _machineInfo.Phone);
                                            ticketModel.FootItemList = footItemList;
                                            string titleStr = string.Empty;
                                            string printStr = printer.GetPrintStrByRecharge(ticketModel, out titleStr);
                                            PrintHelper.Print(printStr, titleStr);

                                        });
                                    }

                                }
                                else
                                {
                                    Toast.ShowMessage(result.Message, CurrentWindow);
                                    if (o1.Equals("1"))
                                    {
                                        YuE -= Amount;
                                    }
                                    else
                                    {
                                        YuE += Amount;
                                    }
                                }
                            }));
                        });

                    }, new Point(200, 300));


                });
            }
        }
    }
}
