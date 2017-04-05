﻿using Pharos.POS.Retailing.Models.ApiParams;
using Pharos.POS.Retailing.Models.ApiReturnResults;
using Pharos.POS.Retailing.Models.PosModels;
using Pharos.Wpf.ViewModelHelpers;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class ProductEdit : BaseViewModel
    {
        public ProductEdit(Product product)
        {
            CurrentProduct = product;

            Barcode = product.Barcode;
            Title = product.Title;
            SysPrice = product.Price;
            Num = product.Number;
            Preferential = product.ActualPrice;
            Status = product.Status;
            EnableEditNum = !product.EnableEditNum;
            EnableEditPrice = !product.EnableEditPrice;

            this.PropertyChanged += ProductEdit_PropertyChanged;

        }
        private Product CurrentProduct { get; set; }
        /// <summary>
        /// 是否允许编辑数量
        /// </summary>
        private bool enableEditNum;

        public bool EnableEditNumForButton
        {
            get
            {
                return !enableEditNum;
            }
        }
        public bool EnableEditNum
        {
            get { return enableEditNum; }
            set
            {
                enableEditNum = value;
                this.OnPropertyChanged(o => o.EnableEditNum);
                this.OnPropertyChanged(o => o.EnableEditNumForButton);
            }
        }
        /// <summary>
        /// 是否允许编辑价格
        /// </summary>
        private bool enableEditPrice;

        public bool EnableEditPrice
        {
            get { return enableEditPrice; }
            set { enableEditPrice = value; this.OnPropertyChanged(o => o.EnableEditPrice); }
        }


        SaleStatus status;
        public SaleStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                this.OnPropertyChanged(o => o.Status);
            }
        }

        string fristProperty = string.Empty;
        void ProductEdit_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == this.GetPropertyName(o => o.Num) && string.IsNullOrEmpty(fristProperty))
                {
                    //Total
                    fristProperty = e.PropertyName;
                    Total = Num * Preferential;
                    fristProperty = string.Empty;
                }
                else if (e.PropertyName == this.GetPropertyName(o => o.Discount) && string.IsNullOrEmpty(fristProperty))
                {
                    //Preferential
                    //Discount = Math.Round(Discount, 2, MidpointRounding.AwayFromZero);
                    fristProperty = e.PropertyName;
                    Preferential = SysPrice * (Discount / 10);
                    Total = Num * Preferential;
                    fristProperty = string.Empty;
                }
                else if (e.PropertyName == this.GetPropertyName(o => o.Preferential) && string.IsNullOrEmpty(fristProperty))
                {
                    fristProperty = e.PropertyName;
                    Discount = Math.Round(Preferential / SysPrice * 10, 2, MidpointRounding.AwayFromZero);
                    Total = Num * Preferential;
                    fristProperty = string.Empty;
                }
                else if (e.PropertyName == this.GetPropertyName(o => o.Total) && string.IsNullOrEmpty(fristProperty))
                {
                    fristProperty = e.PropertyName;
                    Preferential = Total / Num;
                    Discount = Math.Round(Preferential / SysPrice * 10, 2, MidpointRounding.AwayFromZero);
                    fristProperty = string.Empty;
                }
            }
            catch { }
        }
        /// <summary>
        /// 条码
        /// </summary>
        private string barcode;

        public string Barcode
        {
            get { return barcode; }
            set { barcode = value; this.OnPropertyChanged(o => o.Barcode); }
        }
        /// <summary>
        /// 标题
        /// </summary>
        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; this.OnPropertyChanged(o => o.Title); }
        }
        /// <summary>
        /// 数量
        /// </summary>
        private decimal num;

        public decimal Num
        {
            get { return num; }
            set { num = value; this.OnPropertyChanged(o => o.Num); }
        }

        private decimal sysPrice = 100.00m;

        public decimal SysPrice
        {
            get { return sysPrice; }
            set { sysPrice = value; }
        }



        /// <summary>
        /// 折扣
        /// </summary>
        private decimal dicount;

        public decimal Discount
        {
            get { return dicount; }
            set
            {
                dicount = Math.Round(value, 2, MidpointRounding.AwayFromZero);
                if (Discount <= 0)
                {
                    Discount = 0.1m;
                }
                if (Discount > 10)
                {
                    Discount = 10m;
                }
                this.OnPropertyChanged(o => o.Discount);
            }
        }
        /// <summary>
        /// 折后/特价
        /// </summary>
        private decimal preferential;

        public decimal Preferential
        {
            get { return preferential; }
            set
            {
                preferential = value;
                if (Preferential < 0)
                {
                    Preferential = 0.01m;
                }
                this.OnPropertyChanged(o => o.Preferential);
            }
        }

        private decimal total;
        /// <summary>
        /// 小计
        /// </summary>
        public decimal Total
        {
            get { return total; }
            set { total = value; this.OnPropertyChanged(o => o.Total); }
        }

        public ICommand NumAdd
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    Num += 1;
                });
            }
        }
        public ICommand NumDec
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    Num -= 1;
                    if (Num < 1)
                    {
                        Num = 1;
                    }
                });
            }
        }

        public ICommand Confirm
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    Task.Factory.StartNew(() =>
                    {
                        var _machineInfo = Global.MachineSettings.MachineInformations;
                        SaleParams _params = new SaleParams()
                        {
                            StoreId = _machineInfo.StoreId,
                            MachineSn = _machineInfo.MachineSn,
                            Barcode = Barcode,
                            Status = Status,
                            Number = Num,
                            SalePrice = Preferential,
                            HasEditPrice = CurrentProduct.HasEditPrice,
                            RecordId = CurrentProduct.RecordId,
                            CID = _machineInfo.CompanyId
                        };
                        var result = ApiManager.Post<SaleParams, ApiRetrunResult<ApiSaleReturn>>(@"api/SaleOrderEdit", _params);//POST /api/SaleOrderEdit

                        if (result.Code == "200")
                        {
                            CurrentWindow.Dispatcher.Invoke(new Action(() =>
                            {
                                PosViewModel.Current.OrderList = result.Result.BuyList;
                                PosViewModel.Current.Preferential = result.Result.Statistics.Preferential;
                                PosViewModel.Current.ManJianPreferential = result.Result.Statistics.ManJianPreferential;
                                PosViewModel.Current.Receivable = result.Result.Statistics.Receivable;
                                PosViewModel.Current.Num = result.Result.Statistics.Num;
                                PosViewModel.Current.GetPreferentialTitle(result.Result.Statistics.ManYuanPreferential, result.Result.Statistics.ZuHePreferential, result.Result.Statistics.ManJianPreferential);
                                CurrentWindow.Close();
                            }));
                        }
                        else
                        {
                            CurrentWindow.Dispatcher.Invoke(new Action(() =>
                            {
                                Toast.ShowMessage(result.Message, CurrentWindow);
                            }));
                        }
                    });
                });
            }
        }


    }
}