using Pharos.POS.Retailing.Models.ApiParams;
using Pharos.POS.Retailing.Models.ApiReturnResults;
using Pharos.Wpf.ViewModelHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class OrderDetailsViewModel : BaseViewModel
    {
        public OrderDetailsViewModel(string paySn)
        {
            Task.Factory.StartNew(() =>
            {
                MachineInformations _machinesInfo = Global.MachineSettings.MachineInformations;
                FindBillHistoryParams _params = new FindBillHistoryParams()
                {
                    StoreId = _machinesInfo.StoreId,
                    MachineSn = _machinesInfo.MachineSn,
                    CID = _machinesInfo.CompanyId,
                    PaySn = paySn
                };
                var result = ApiManager.Post<FindBillHistoryParams, ApiRetrunResult<BillHistoryInfo>>(@"api/FindBillHistory", _params);
                if (result.Code == "200")
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        var orderTypeInt = result.Result.OrderType;
                        var status = result.Result.OrderStatus;//type=0正常 1换货 2退货    status=0正常 1退单
                        if (status == 1)
                        {
                            OrderType = "退单";
                        }
                        else
                        {
                            if (orderTypeInt == 1 || orderTypeInt == 2)
                            {
                                OrderType = "退换";
                            }
                            else
                            {
                                OrderType = "销售";
                            }
                        }
                        PaySn = result.Result.PaySn;
                        CreateDT = result.Result.OrderTime;
                        PayType = result.Result.Payment;
                        CashierName = result.Result.CashierName;
                        SaleManName = result.Result.SaleManName;
                        TotalAmount = result.Result.WipeZeroAfterTotalAmount;
                        WipeZero = result.Result.WipeZero;
                        PreferentialAmount = result.Result.PreferentialAmount;
                        Unit = result.Result.ProductCount;
                        OrderItems = result.Result.Details;
                        OldOrderSN = result.Result.OldOrderSN;
                        OrderDiscount = -result.Result.OrderDiscount;//2016-08-03 整单让利数据源变成正的加-
                    }));

                }
                else
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        Toast.ShowMessage(result.Message, CurrentWindow);
                    }));

                }
            });
        }

        private IEnumerable<BillDetails> orderItems;

        public IEnumerable<BillDetails> OrderItems
        {
            get { return orderItems; }
            set
            {
                orderItems = value;
                var count = orderItems.Count();
                for (var i = 0; i < count; i++)
                {
                    var item = orderItems.ElementAt(i);
                    item.Index = (i + 1);
                    //item.Parent = this;
                }

                this.OnPropertyChanged(o => o.OrderItems);
            }
        }

        private string oldOrderSN;
        /// <summary>
        /// 原流水号
        /// </summary>
        public string OldOrderSN
        {
            get { return oldOrderSN; }
            set
            {
                oldOrderSN = value;
                this.OnPropertyChanged(o => o.OldOrderSN);
            }
        }



        private decimal orderDiscount;

        public decimal OrderDiscount
        {
            get { return orderDiscount; }
            set { orderDiscount = value; this.OnPropertyChanged(o => o.OrderDiscount); }
        }

        /// <summary>
        /// 订单类型
        /// </summary>
        //private int orderTypeInt;

        //public int OrderTypeInt
        //{
        //    get { return orderTypeInt; }
        //    set { orderTypeInt = value; }
        //}

        //private int orderStatus;

        //public int OrderStatus
        //{
        //    get { return orderStatus; }
        //    set { orderStatus = value; }
        //}
        private string orderType;

        public string OrderType
        {
            get { return orderType; }
            set
            {
                orderType = value;
                this.OnPropertyChanged(o => o.OrderType);
            }
        }
        private string paySn;

        public string PaySn
        {
            get { return paySn; }
            set { paySn = value; this.OnPropertyChanged(o => o.PaySn); }
        }
        private DateTime createDT;

        public DateTime CreateDT
        {
            get { return createDT; }
            set { createDT = value; this.OnPropertyChanged(o => o.CreateDT); }
        }

        private string payType;

        public string PayType
        {
            get { return payType; }
            set { payType = value; this.OnPropertyChanged(o => o.PayType); }
        }

        private string cashierName;

        public string CashierName
        {
            get { return cashierName; }
            set { cashierName = value; this.OnPropertyChanged(o => o.CashierName); }
        }

        private string saleManName;

        public string SaleManName
        {
            get { return saleManName; }
            set { saleManName = value; this.OnPropertyChanged(o => o.SaleManName); }
        }

        private decimal totalAmount;

        public decimal TotalAmount
        {
            get { return totalAmount; }
            set { totalAmount = value; this.OnPropertyChanged(o => o.TotalAmount); }
        }
        private decimal wipeZero;

        public decimal WipeZero
        {
            get { return wipeZero; }
            set { wipeZero = value; this.OnPropertyChanged(o => o.WipeZero); }
        }
        private decimal preferentialAmount;

        public decimal PreferentialAmount
        {
            get { return preferentialAmount; }
            set { preferentialAmount = value; this.OnPropertyChanged(o => o.PreferentialAmount); }
        }

        private decimal unit;

        public decimal Unit
        {
            get { return unit; }
            set { unit = value; this.OnPropertyChanged(o => o.Unit); }
        }
    }

}
