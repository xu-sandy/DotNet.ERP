﻿using Pharos.POS.Retailing.ChildWin;
using Pharos.POS.Retailing.Models.ApiParams;
using Pharos.POS.Retailing.Models.ApiReturnResults;
using Pharos.Wpf.ViewModelHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    /// <summary>
    /// 查询历史订单
    /// </summary>
    public class FindBills : BaseViewModel
    {
        public FindBills()
        {
            Task.Factory.StartNew(() =>
            {
                searchTime = DateTime.Now;
              //  SearchCommand.Execute(Range.Local);
            });
        }

        private DateTime searchTime = DateTime.Now;

        public DateTime SearchTime
        {
            get { return searchTime; }
            set { searchTime = value; }
        }
        /// <summary>
        /// 流水号
        /// </summary>
        private string paySn;

        public string PaySn
        {
            get { return paySn; }
            set { paySn = value; }
        }

        private string cashier = Global.CurrentSaleMen.UserCode;

        public string Cashier
        {
            get { return cashier; }
            set { cashier = value; this.OnPropertyChanged(o => o.Cashier); }
        }

        private string machineSn = Global.MachineSettings.MachineInformations.MachineSn;

        public string MachineSn
        {
            get { return machineSn; }
            set
            {
                machineSn = value;

                if (!Regex.IsMatch(value, "^[0-9][0-9]$") && CurrentWindow != null && value.Length > 0)
                {
                    Toast.ShowMessage("POS机号限01-99范围！", CurrentWindow);
                    return;
                }
                if (value.Length == 1)
                {
                    machineSn = "0" + value;
                }
                else
                {
                    machineSn = value;
                }

                this.OnPropertyChanged(o => o.MachineSn);
            }
        }



        private IEnumerable<OrderInfoModel> orderItems;

        public IEnumerable<OrderInfoModel> OrderItems
        {
            get { return orderItems; }
            set
            {
                orderItems = value.OrderBy(o => o.Date);
                var count = orderItems.Count();
                for (var i = 0; i < count; i++)
                {
                    var item = orderItems.ElementAt(i);
                    item.Index = (i + 1);
                    item.Parent = this;
                }
                orderItems = orderItems.OrderByDescending(o => o.Index); ;

                this.OnPropertyChanged(o => o.OrderItems);
            }
        }
        public Action Callback;






        public GeneralCommand<Range> SearchCommand
        {
            get
            {
                return new GeneralCommand<Range>((o1, o2) =>
                {
                    QueryModel.Current.IsQuery = true;
                    OrderItems = new List<OrderInfoModel>();
                    Task.Factory.StartNew(() =>
                    {
                        var _machinesInfo = Global.MachineSettings.MachineInformations;
                        FindBillsParams _params = new FindBillsParams() { Date = SearchTime, Range = o1, StoreId = _machinesInfo.StoreId, MachineSn = _machinesInfo.MachineSn, QueryMachineSn = MachineSn, CID = _machinesInfo.CompanyId, PaySn = PaySn, Cashier = Cashier };
                        var result = ApiManager.Post<FindBillsParams, ApiRetrunResult<IEnumerable<OrderInfoModel>>>(@"api/FindBills", _params);
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            if (result.Code == "200")
                            {
                                OrderItems = result.Result.OrderByDescending(o => o.Date);
                            }
                            else
                            {
                                Toast.ShowMessage(result.Message, CurrentWindow);
                            }
                            QueryModel.Current.IsQuery = false;

                        }));

                        if (Callback != null)
                        {
                            Callback();
                        }

                    });
                });
            }
        }
    }
}