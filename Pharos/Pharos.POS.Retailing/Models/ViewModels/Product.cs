﻿using Newtonsoft.Json;
using Pharos.POS.Retailing.ChildWin;
using Pharos.POS.Retailing.Models.ApiParams;
using Pharos.POS.Retailing.Models.ApiReturnResults;
using Pharos.POS.Retailing.Models.PosModels;
using Pharos.Wpf.ViewModelHelpers;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class Product : BaseViewModel,IEdit
    {
        public string Barcode { get; set; }
        public bool IsMultiCode { get; set; }
        public string Title { get; set; }
        public string Size { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        /// <summary>
        /// 单品优惠价
        /// </summary>
        [JsonIgnore]
        public decimal Preferential { get { return (Total - (Price * Number)) > 0 ? 0 : Math.Abs(Total - (Price * Number)); } }
        public decimal ActualPrice { get; set; }
        public decimal Number { get; set; }
        public bool EnableEditNum { get; set; }
        public bool EnableEditPrice { get; set; }
        public SaleStatus Status { get; set; }
        public decimal Total { get; set; }
        public string GiftId { get; set; }
        public string GiftPromotionId { get; set; }

        public Product Current { get { return this; } }

        public StateToImg ProductStatus { get; set; }

        [JsonIgnore]
        public int Index { get; set; }

        [JsonIgnore]
        public ICommand RemoveCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    PosViewModel.Current.FocusBarcodeControl();

                    Task.Factory.StartNew(() =>
                    {

                        //sale
                        // PosViewModel.Current.OrderList
                        var index = PosViewModel.Current.OrderList.IndexOf(this);

                        var _machineInfo = Global.MachineSettings.MachineInformations;
                        SaleParams _params = new SaleParams()
                        {
                            StoreId = _machineInfo.StoreId,
                            MachineSn = _machineInfo.MachineSn,
                            Barcode = Barcode,
                            Status = Status,
                            HasEditPrice = HasEditPrice,
                            RecordId = RecordId,
                            CID = _machineInfo.CompanyId
                        };
                        var result = ApiManager.Post<SaleParams, ApiRetrunResult<ApiSaleReturn>>(@"api/SaleOrderRemove", _params);//POST /api/SaleOrderRemove

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


                            }));
                            if (PosViewModel.Current.OrderList.Count > 5)
                            {
                                PosViewModel.Current.SetScrollIntoView(index - 1);

                            }
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


        public ICommand EditCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    PosViewModel.Current.FocusBarcodeControl();

                    var index = PosViewModel.Current.OrderList.IndexOf(this);
                    ProductEditor page = new ProductEditor(new ProductEdit(this), 0);
                    page.Owner = CurrentWindow;
                    page.ShowDialog();
                    if (index >= 0)
                        PosViewModel.Current.SetScrollIntoView(index);
                });
            }
        }
        public ICommand EditNumCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    PosViewModel.Current.FocusBarcodeControl();

                    var index = PosViewModel.Current.OrderList.IndexOf(this);

                    ProductEditor page = new ProductEditor(new ProductEdit(this), 1);
                    page.Owner = CurrentWindow;
                    page.ShowDialog();
                    if (index >= 0)
                        PosViewModel.Current.SetScrollIntoView(index);
                });
            }
        }


        public bool HasEditPrice { get; set; }

        public string RecordId { get; set; }
    }
}
