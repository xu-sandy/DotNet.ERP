﻿using Pharos.POS.Retailing.ChildWin;
using Pharos.POS.Retailing.Models.ApiParams;
using Pharos.POS.Retailing.Models.ApiReturnResults;
using Pharos.POS.Retailing.Models.PosModels;
using Pharos.Wpf.ViewModelHelpers;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Pharos.POS.Retailing.Devices.POSDevices;
using System.Threading.Tasks;
using System.Threading;
using Pharos.POS.Retailing.Devices.StoredValueCardDevice;
using Pharos.POS.Retailing.Devices.QuickConnectTools;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Drawing;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.IO.Ports;
using Newtonsoft.Json.Linq;

namespace Pharos.POS.Retailing.Models.ViewModels.Pay
{
    public class MultiPayViewModel : BaseViewModel
    {
        bool isWipeZero = Global.MachineSettings.MachineInformations.IsNonCashWipeZero;
        public MultiPayViewModel(decimal _amount, PayAction _payAction, int reason = 0)
        {
            Amount = _amount;
            PayAction = _payAction;
            Reason = reason;
            if (isWipeZero)
            {
                WipeZeroAfter = Math.Round(_amount, 1, MidpointRounding.AwayFromZero);
            }
            else
            {
                WipeZeroAfter = _amount;
            }
            WipeZero = WipeZeroAfter - _amount;//摸零金额
            StillOwe = WipeZeroAfter;
        }

        private MultiPayItemViewModel _CurrentPayItem;
        public MultiPayItemViewModel CurrentPayItem
        {
            get { return _CurrentPayItem; }
            set
            {
                _CurrentPayItem = value;
                this.OnPropertyChanged(o => o.CurrentPayItem);
            }
        }
        private decimal wipeZeroAfter;
        /// <summary>
        /// 抹零后金额
        /// </summary>
        public decimal WipeZeroAfter
        {
            get { return wipeZeroAfter; }
            set
            {
                wipeZeroAfter = value;
                this.OnPropertyChanged(o => o.WipeZeroAfter);
            }
        }
        private decimal wipeZero;

        /// <summary>
        /// 抹零金额
        /// </summary>
        public decimal WipeZero
        {
            get { return wipeZero; }
            set { wipeZero = value; this.OnPropertyChanged(o => o.WipeZero); }
        }
        int Reason { get; set; }

        private PayAction payAction;
        /// <summary>
        /// 付款类型
        /// </summary>
        public PayAction PayAction
        {
            get { return payAction; }
            set { payAction = value; }
        }

        private decimal amount;
        /// <summary>
        /// 摸零前应收金额
        /// </summary>
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; this.OnPropertyChanged(o => o.Amount); }
        }
        /// <summary>
        /// 尚欠
        /// </summary>
        private decimal stillOwe = 0m;

        public decimal StillOwe
        {
            get { return stillOwe; }
            set { stillOwe = value; this.OnPropertyChanged(o => o.StillOwe); }
        }
        /// <summary>
        /// 找零
        /// </summary>
        private decimal change = 0m;

        public decimal Change
        {
            get { return change; }
            set
            {
                change = value;
                PosViewModel.Current.Change = value;
                this.OnPropertyChanged(o => o.Change);
            }
        }
        private decimal _Received;
        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal Received
        {
            get { return _Received; }
            set { _Received = value; this.OnPropertyChanged(o => o.Received); }
        }
        public ObservableCollection<MultiPayItemViewModel> PayItems { get; set; }

        public bool isComplate = false;
        /// <summary>
        /// 完成支付
        /// </summary>
        public ICommand ConfirmCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    if (StillOwe != 0)
                    {
                        Toast.ShowMessage("未支付金额，请选择支付方式完成支付！", CurrentWindow);
                        return;
                    }
                    if (wipeZeroAfter != Received && wipeZeroAfter < 0)
                    {
                        Toast.ShowMessage("退还金额不正确！", CurrentWindow);
                        return;
                    }
                    if (wipeZeroAfter > Received && wipeZeroAfter >= 0)
                    {
                        Toast.ShowMessage("收款金额不可小于应收金额！", CurrentWindow);
                        return;
                    }
                    if (CurrentPayItem != null && CurrentPayItem.HasOperat)
                    {
                        Toast.ShowMessage(CurrentPayItem.RequestOperatMessage, CurrentWindow);
                        return;
                    }
                    Confirm.ShowMessage(string.Format("请确认是否完成{0}？", Amount >= 0 ? "收款" : "退款"), Application.Current.MainWindow, (mode) =>
                    {
                        if (mode == ConfirmMode.Cancelled) return;
                        MachineInformations _machineInfo = Global.MachineSettings.MachineInformations;
                        //判读是否有用该支付方式支付
                        List<PayWay> payWay = new List<PayWay>();
                        foreach (var item in PayItems)
                        {
                            if (item.PayItem == null) continue;
                            PayWay _payWay = new PayWay()
                            {
                                Amount = item.Amount,
                                Change = 0m,
                                Receive = item.Amount,
                                Type = item.PayItem.Mode,
                                WipeZero = 0m,
                                CardNo = item.CardNo
                            };
                            if (item.PayItem.Mode == PayMode.CashPay)
                            {
                                _payWay.WipeZero = WipeZero;
                                _payWay.Change = Change;
                            }
                            payWay.Add(_payWay);
                        }
                        ApiPayParams _params = new ApiPayParams()
                        {
                            StoreId = _machineInfo.StoreId,
                            MachineSn = _machineInfo.MachineSn,
                            CID = _machineInfo.CompanyId,
                            Mode = PayAction,
                            Receivable = WipeZeroAfter,
                            Payway = payWay,
                            OrderAmount = Amount,
                            Reason = 0,
                            OldOrderSn = (PayAction == PayAction.RefundAll ? RefundChangeViewModel.Current.RefundOrder.PaySn : "")
                        };
                        var result = ApiManager.Post<ApiPayParams, ApiRetrunResult<object>>(@"api/Pay", _params);
                        if (result.Code == "200")
                        {
                            isComplate = true;
                            //(payWay.Select(o => o.Type).Distinct().Count() == 1 ? PayItems.FirstOrDefault(o => o.PayItem != null).PayName : "多方付")
                            //返回成功则打开微信支付界面
                            ZhiFuWanCheng page = new ZhiFuWanCheng(WipeZeroAfter, Received, Change, PayItems.Where(o => o.PayItem != null).ToList(), PayAction, (DateTime)result.Result);//支付方式参数
                            page.Owner = Application.Current.MainWindow;
                            CurrentWindow.Close();
                            page.ShowDialog();
                        }
                        else
                        {
                            Toast.ShowMessage(result.Message, CurrentWindow);
                        }
                    });
                });

            }
        }
    }
    public class MultiPayItemViewModel : BaseViewModel, Pharos.POS.Retailing.XamlConverters.IMultiPayViewModel
    {
        public MultiPayItemViewModel()
        {
            IsRequestEnd = true;
            this.OnPropertyChanged(o => o.IsRequestEnd);

        }

        public bool IsLast { get; set; }
        public bool IsFrist { get; set; }
        public bool IsSelected { get; set; }
        public decimal Amount { get; set; }
        public string PayName { get; set; }
        PayItem _PayItem;
        public PayItem PayItem
        {
            get { return _PayItem; }
            set
            {
                _PayItem = value;
                EnableClose = true;
                if (_PayItem == null)
                {
                    RequestOperatMessage = string.Empty;
                    HasOperat = false;
                }
                else
                {
                    switch (_PayItem.Mode)
                    {
                        case PayMode.StoredValueCard:
                            RequestOperatMessage = "请刷储值卡或者输入储值卡号！";
                            HasOperat = true;
                            break;
                        case PayMode.UnionPayCTPOSM:
                            RequestOperatMessage = "请刷银联卡进行支付！";
                            HasOperat = false;
                            break;
                        case PayMode.RongHeCustomerDynamicQRCodePay:
                            HasOperat = true;
                            RequestOperatMessage = "请使用扫码枪扫描客户手机支付条码/二维码！";
                            break;
                        default:
                            HasOperat = false;
                            RequestOperatMessage = string.Empty;
                            break;
                    }
                }
            }
        }
        string _CardNo;
        public string CardNo
        {
            get { return _CardNo; }
            set
            {
                if (value == null)
                    value = "";
                _CardNo = value.Trim();
                this.OnPropertyChanged(o => o.CardNo);
            }
        }
        string _CardName;
        public string CardName
        {
            get { return _CardName; }
            set
            {
                _CardName = value;
                this.OnPropertyChanged(o => o.CardName);
            }
        }
        string _Message;
        public string Message
        {
            get { return _Message; }
            set
            {
                _Message = value;
                this.OnPropertyChanged(o => o.Message);
            }
        }

        private string PayOrderSn { get; set; }
        public void Cannel()
        {
            switch (PayItem.Mode)
            {
                case PayMode.UnionPayCTPOSM:
                    {
                        if (source != null)
                        {
                            source.Cancel();
                        }
                    }
                    break;
            }
        }
        internal CancellationToken token;
        internal CancellationTokenSource source = null;
        internal bool isRetry = false;
        public bool IsRequestEnd { get; set; }
        public void DoPay()
        {
            IsRequestEnd = false;
            this.OnPropertyChanged(o => o.IsRequestEnd);

            switch (PayItem.Mode)
            {
                case PayMode.StoredValueCard:
                    {

                        Task.Factory.StartNew(() =>
                       {
                           bool isSuccess;
                           var password = string.Empty;
                           if (string.IsNullOrEmpty(CardNo) || isRetry)//将来设备完整后要去掉
                           {

                               if (source != null)
                               {
                                   source.Cancel();
                                   Thread.Sleep(200);
                               }

                               source = new CancellationTokenSource();
                               token = source.Token;
                               Application.Current.Dispatcher.Invoke(new Action(() =>
                               {
                                   Message = "请操作读卡设备进行收款！【如果设备未响应，请检查设备连接并按“F5”重试】";
                                   IsSuccess = null;
                                   IsRunning = true;
                               }));

                               var device = StoredValueCardDeviceFactory.Create();
                               StoreValueCardInfomactions info;
                               string msg;
                               isSuccess = device.ReadCard(token, Amount, out info, out msg);
                               source = null;
                               Application.Current.Dispatcher.Invoke(new Action(() =>
                               {
                                   IsSuccess = isSuccess;
                                   if (!isSuccess)
                                   {
                                       isRetry = false;
                                       Message = msg + "【按F5重新发起支付】";
                                   }
                                   else
                                   {
                                       Message = msg + "正在等待后台回应！";
                                   }
                                   if (info != null)
                                   {
                                       CardNo = info.CardNo;
                                       password = info.Password;
                                   }
                                   else
                                   {
                                       CardNo = string.Empty;
                                       password = string.Empty;
                                   }
                                   IsRunning = false;
                                   IsRequestEnd = true;
                                   this.OnPropertyChanged(o => o.IsRequestEnd);

                               }));
                           }
                           else
                           {
                               isSuccess = true;
                           }
                           if (isSuccess)
                           {
                               Application.Current.Dispatcher.Invoke(new Action(() =>
                                   {
                                       MachineInformations _machineInfo = Global.MachineSettings.MachineInformations;
                                       BackgroundPaymentParams _params = new BackgroundPaymentParams()
                                       {
                                           StoreId = _machineInfo.StoreId,
                                           MachineSn = _machineInfo.MachineSn,
                                           CID = _machineInfo.CompanyId,
                                           Amount = Amount,
                                           CardNo = _CardNo,
                                           CardPassword = password,
                                           Mode = PayMode.StoredValueCard
                                       };
                                       var result = ApiManager.Post<BackgroundPaymentParams, ApiRetrunResult<StoredValueCardInfo, StoredValueCardInfo>>(@"api/BackgroundPayment", _params);
                                       if (result.Code == "200")
                                       {
                                           HasOperat = false;
                                           Balance = result.Result.Balance;
                                           User = result.Result.User;
                                           PayAmount = result.Result.PayAmount;
                                           this.OnPropertyChanged(o => o.HasOperat);
                                           this.OnPropertyChanged(o => o.Balance);
                                           this.OnPropertyChanged(o => o.PayAmount);
                                           this.OnPropertyChanged(o => o.User);
                                           this.OnPropertyChanged(o => o.CardNo);
                                           isRetry = false;
                                           EnableClose = false;
                                           Message = string.Format("成功支付{0:N}元！", Amount);
                                       }
                                       else
                                       {
                                           if (result.ErrorInfo != null)
                                           {
                                               Balance = result.ErrorInfo.Balance;
                                               User = result.ErrorInfo.User;
                                               PayAmount = result.ErrorInfo.PayAmount;
                                               this.OnPropertyChanged(o => o.Balance);
                                               this.OnPropertyChanged(o => o.PayAmount);
                                               this.OnPropertyChanged(o => o.CardNo);
                                               this.OnPropertyChanged(o => o.User);
                                           }
                                           isRetry = false;
                                           IsSuccess = false;
                                           Message = result.Message + "【按F5重新发起支付】";
                                       }
                                       IsRequestEnd = true;
                                       this.OnPropertyChanged(o => o.IsRequestEnd);

                                   }));
                           }
                       });
                    }
                    break;
                case PayMode.UnionPayCTPOSM:
                    {


                        Task.Factory.StartNew(() =>
                        {
                            if (source != null)
                            {
                                source.Cancel();
                                Thread.Sleep(500);
                            }
                            source = new CancellationTokenSource();
                            token = source.Token;
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                              {
                                  Message = "请操作POS银联机进行收款！【如果设备未响应，请检查设备连接并按“F5”重试】";
                                  IsSuccess = null;
                                  IsRunning = true;
                              }));
                            var posDevice = POSDeviceFactory.Create();
                            string msg;
                            var request = new POSDevicePayRequest()
                            {
                                Amount = Amount,
                                CashierId = PosViewModel.Current.UserCode,
                                MachineSn = Global.MachineSettings.MachineInformations.MachineSn,
                                OldTransactionCode = "",
                                OrderSn = PosViewModel.Current.OrderSn,
                                Type = Amount >= 0 ? TransactionType.Consumption : TransactionType.Refund
                            };
                            POSDevicePayResponse response;
                            var isSuccess = posDevice.DoPay(token, request, out response, out msg);
                            source = null;
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                               {
                                   IsSuccess = isSuccess;
                                   EnableClose = !isSuccess;
                                   if (!isSuccess)
                                   {
                                       msg += "【按F5重新发起支付】";
                                   }
                                   else
                                   {
                                       HasOperat = false;
                                   }
                                   Message = msg;
                                   if (response != null)
                                   {
                                       CardNo = response.CardNo;
                                       CardName = response.CardName;
                                   }
                                   IsRunning = false;
                                   IsRequestEnd = true;
                                   this.OnPropertyChanged(o => o.IsRequestEnd);

                               }));
                        });
                    }
                    break;
                case PayMode.RongHeDynamicQRCodePay:
                    IsSuccess = null;
                    Message = "正在请求支付，请稍后！";
                    Task.Factory.StartNew(() =>
                       {
                           if (source != null)
                           {
                               source.Cancel();
                               Thread.Sleep(500);
                           }
                           source = new CancellationTokenSource();
                           token = source.Token;
                           MachineInformations _machineInfo = Global.MachineSettings.MachineInformations;
                           BackgroundPaymentParams _params = new BackgroundPaymentParams()
                           {
                               StoreId = _machineInfo.StoreId,
                               MachineSn = _machineInfo.MachineSn,
                               CID = _machineInfo.CompanyId,
                               Amount = Amount,
                               CardNo = _CardNo,
                               CardPassword = string.Empty,
                               Mode = PayMode.RongHeDynamicQRCodePay
                           };
                           var result = ApiManager.Post<BackgroundPaymentParams, ApiRetrunResult<JObject>>(@"api/BackgroundPayment", _params);
                           Application.Current.Dispatcher.Invoke(new Action(() =>
                           {
                               if (result.Code == "200")
                               {
                                   HasOperat = false;
                                   var qrCodeEncoder = new QrEncoder();
                                   var qrCode = qrCodeEncoder.Encode(result.Result.Property("PayToken").ToString());
                                   var renderer = new GraphicsRenderer(new FixedModuleSize(60, QuietZoneModules.Zero), Brushes.Black, Brushes.White);
                                   PayOrderSn = result.Result.Property("PayOrderSn").ToString();
                                   using (var stream = new MemoryStream())
                                   {
                                       renderer.WriteToStream(qrCode.Matrix, ImageFormat.Bmp, stream);
                                       BitmapImage bitmapImage = new BitmapImage();
                                       bitmapImage.BeginInit();
                                       bitmapImage.StreamSource = stream;
                                       bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                                       bitmapImage.EndInit();
                                       bitmapImage.Freeze();
                                       PayItem.RongHeDynamicQRCode = bitmapImage;
                                   }
                                   PayItem.QrContent = result.Result.Property("PayToken").ToString();
                                   IsSuccess = null;
                                   Message = "请使用微信或者支付宝扫描此二维码进行支付！";
                                   PosViewModel.Current.MultiPayItemViewModel = this;

                                   GetBackgroundPaymentState(_params, token);
                               }
                               else
                               {
                                   IsSuccess = false;
                                   Message = result.Message;
                               }
                               IsRequestEnd = true;
                               this.OnPropertyChanged(o => o.IsRequestEnd);

                           }));
                       });
                    break;
                case PayMode.RongHeCustomerDynamicQRCodePay:
                    IsSuccess = null;
                    Message = "正在请求支付，请稍后！";
                    Task.Factory.StartNew(() =>
                    {
                        if (source != null)
                        {
                            source.Cancel();
                            Thread.Sleep(500);
                        }
                        source = new CancellationTokenSource();
                        token = source.Token;
                        MachineInformations _machineInfo = Global.MachineSettings.MachineInformations;
                        BackgroundPaymentParams _params = new BackgroundPaymentParams()
                        {
                            StoreId = _machineInfo.StoreId,
                            MachineSn = _machineInfo.MachineSn,
                            CID = _machineInfo.CompanyId,
                            Amount = Amount,
                            CardNo = _CardNo,
                            CardPassword = string.Empty,
                            Mode = PayMode.RongHeCustomerDynamicQRCodePay
                        };
                        var result = ApiManager.Post<BackgroundPaymentParams, ApiRetrunResult<string>>(@"api/BackgroundPayment", _params);
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            if (result.Code == "200")
                            {
                                HasOperat = false;
                                EnableClose = false;
                                IsSuccess = true;
                                Message = string.Format("成功支付{0:N2}元！", Amount);
                                PayOrderSn = result.Result;
                                PosViewModel.Current.MultiPayItemViewModel = null;
                            }
                            else
                            {
                                IsSuccess = false;
                                Message = result.Message;
                            }
                            IsRequestEnd = true;
                            this.OnPropertyChanged(o => o.IsRequestEnd);

                        }));
                    });
                    break;
            }
        }

        private Task GetBackgroundPaymentState(BackgroundPaymentParams _params, CancellationToken token)
        {
            return Task.Factory.StartNew(() =>
             {
                 while (true)
                 {

                     var result = ApiManager.Post<BackgroundPaymentParams, ApiRetrunResult<int>>(@"api/GetBackgroundPaymentState", _params);
                     if (token.IsCancellationRequested) { return; }
                     if (result.Code == "200")
                     {
                         if (result.Result == 1)
                         {
                             Application.Current.Dispatcher.Invoke(new Action(() =>
                             {
                                 PosViewModel.Current.MultiPayItemViewModel = null;
                                 IsSuccess = true;
                                 EnableClose = false;
                                 Message = string.Format("成功支付{0:N2}元！", Amount);
                                 return;
                             }));
                         }
                         else if (result.Result == -1)
                         {
                             Application.Current.Dispatcher.Invoke(new Action(() =>
                             {
                                 IsSuccess = false;
                                 Message = "支付失败，请按F5重新发起支付！";
                                 return;
                             }));
                         }
                         else
                         {
                             Thread.Sleep(1000);
                         }
                     }
                 }
             });
        }
        public bool IsRunning { get; set; }
        public decimal Balance { get; set; }
        public string User { get; set; }
        public decimal PayAmount { get; set; }


        private bool _HasOperat;
        public bool HasOperat
        {
            get
            {
                return _HasOperat;
            }
            set
            {
                _HasOperat = value;
                this.OnPropertyChanged(o => o.HasOperat);
            }
        }
        private bool? _IsSuccess = false;
        public bool? IsSuccess
        {
            get
            {
                return _IsSuccess;
            }
            set
            {
                _IsSuccess = value;
                this.OnPropertyChanged(o => o.IsSuccess);
            }
        }
        public string RequestOperatMessage { get; set; }
        private bool _EnableClose = true;
        public bool EnableClose
        {
            get
            {
                return _EnableClose;
            }
            set
            {
                _EnableClose = value;
                this.OnPropertyChanged(o => o.EnableClose);
            }
        }
    }

    public class StoredValueCardInfo
    {
        public decimal Balance { get; set; }
        public string User { get; set; }
        public decimal PayAmount { get; set; }
    }
}
