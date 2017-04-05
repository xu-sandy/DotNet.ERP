﻿using Newtonsoft.Json;
using Pharos.POS.Retailing.Models.ApiParams;
using Pharos.POS.Retailing.Models.ApiReturnResults;
using Pharos.Wpf.ViewModelHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class ApiHandBillListReturn : BaseViewModel
    {
        /// <summary>
        /// 挂单号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 挂单时间
        /// </summary>
        public DateTime SuspendDate { get; set; }
        /// <summary>
        /// 预购商品数量
        /// </summary>
        public decimal Count { get; set; }
        /// <summary>
        /// 预购商品总额
        /// </summary>
        public decimal Amount { get; set; }
        public string OrderSN { get; set; }

        public int Index { get; set; }

        [JsonIgnore]
        public ICommand RemoveCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    Task.Factory.StartNew(() =>
                    {
                        var _machinesInfo = Global.MachineSettings.MachineInformations;
                        HandBillParams _params = new HandBillParams() { StoreId = _machinesInfo.StoreId, MachineSn = _machinesInfo.MachineSn, CID = _machinesInfo.CompanyId, OrderSn = Id };
                        var result = ApiManager.Post<HandBillParams, ApiRetrunResult<IEnumerable<ApiHandBillListReturn>>>(@"api/RemoveHandBill", _params);
                        Task.Factory.StartNew(() =>
                        {
                            if (result.Code == "200")
                            {
                                Application.Current.Dispatcher.Invoke(new Action(() =>
                                {
                                    HandBillList.Current.OrderItems = result.Result;
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
                    });
                });
            }
        }
        public ICommand ReadHandBill
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {

                    var _machinesInfo = Global.MachineSettings.MachineInformations;
                    HandBillParams _params = new HandBillParams() { StoreId = _machinesInfo.StoreId, MachineSn = _machinesInfo.MachineSn, CID = _machinesInfo.CompanyId, OrderSn = Id };
                    Task.Factory.StartNew(() =>
                    {
                        var result = ApiManager.Post<HandBillParams, ApiRetrunResult<ApiSaleReturn>>(@"api/ReadHandBill", _params);

                        if (result.Code == "200")
                        {
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                PosViewModel.Current.OrderList = result.Result.BuyList;
                                PosViewModel.Current.Preferential = result.Result.Statistics.Preferential;
                                PosViewModel.Current.ManJianPreferential = result.Result.Statistics.ManJianPreferential;
                                PosViewModel.Current.Receivable = result.Result.Statistics.Receivable;
                                PosViewModel.Current.Num = result.Result.Statistics.Num;
                               // CurrentWindow.DialogResult = true;
                                CurrentWindow.Close();
                            }));
                            //HandBillList.Current.OrderItems = result.Result;

                        }
                        else
                        {
                            Application.Current.Dispatcher.Invoke(new Action(() =>
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
