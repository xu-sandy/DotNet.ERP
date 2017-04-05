﻿using Pharos.POS.Retailing.Models.ApiParams;
using Pharos.POS.Retailing.Models.ApiReturnResults;
using Pharos.Wpf.ViewModelHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    /// <summary>
    /// 挂单清单
    /// </summary>
    public class HandBillList : BaseViewModel
    {
        public HandBillList()
        {
            Current = this;
            try
            {
                QueryModel.Current.IsQuery = true;

                var _machinesInfo = Global.MachineSettings.MachineInformations;
                Task.Factory.StartNew(() =>
                {
                    BaseApiParams _params = new BaseApiParams() { StoreId = _machinesInfo.StoreId, MachineSn = _machinesInfo.MachineSn, CID = _machinesInfo.CompanyId };
                    var result = ApiManager.Post<BaseApiParams, ApiRetrunResult<IEnumerable<ApiHandBillListReturn>>>(@"api/HandBillList", _params);

                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        if (result.Code == "200")
                        {

                            OrderItems = result.Result.OrderByDescending(o => o.SuspendDate);
                            foreach (var item in orderItems)
                            {
                                item.CurrentWindow = CurrentWindow;
                                item.CurrentBindingElement = CurrentBindingElement;
                            }
                        }
                        else
                        {
                            Toast.ShowMessage(result.Message, CurrentWindow);
                        }
                        QueryModel.Current.IsQuery = false;

                    }));
                });
            }
            catch (Exception ex)
            {
                Toast.ShowMessage(ex.Message, CurrentWindow);
            }

        }
        public static HandBillList Current { get; private set; }
        /// <summary>
        /// 网格双向绑定值
        /// </summary>
        private IEnumerable<ApiHandBillListReturn> orderItems;

        public IEnumerable<ApiHandBillListReturn> OrderItems
        {
            get { return orderItems; }
            set
            {
                orderItems = value;
                var count = value.Count();
                for (var i = 0; i < count; i++)
                {
                    var item = orderItems.ElementAt(i);
                    item.CurrentWindow = CurrentWindow;
                    item.Index = i + 1;
                }
                this.OnPropertyChanged(o => o.OrderItems);
            }
        }





    }
}
