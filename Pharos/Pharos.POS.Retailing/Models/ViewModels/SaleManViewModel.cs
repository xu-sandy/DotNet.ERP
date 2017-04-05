﻿using Pharos.POS.Retailing.Models.ApiParams;
using Pharos.POS.Retailing.Models.ApiReturnResults;
using Pharos.Wpf.ViewModelHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class SaleManViewModel : BaseViewModel
    {
        MachineInformations _machineInfo = Global.MachineSettings.MachineInformations;

        public static SaleManViewModel Current { get; private set; }
        public SaleManViewModel(int _pageSource)
        {
            pageSource = _pageSource;
            Current = this;
            //获取所有导购员
            Task.Factory.StartNew(() =>
            {
                UserInfoParams _params = new UserInfoParams
                {
                    CID = _machineInfo.CompanyId,
                    StoreId = _machineInfo.StoreId,
                    MachineSn = _machineInfo.MachineSn,
                    StoreOperateAuth = StoreOperateAuth.ShoppingGuide
                };
                var result = ApiManager.Post<UserInfoParams, ApiRetrunResult<ObservableCollection<UserInfoResult>>>(@"api/GetAuthUsers", _params);
                if (result.Code == "200")
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                     {
                         UserInfo = result.Result;
                     }));
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        Toast.ShowMessage(result.Message, Application.Current.MainWindow);
                    }));
                }
            });
        }
        private int pageSource = 0;
        /// <summary>
        /// 导购员列表
        /// </summary>
        private ObservableCollection<UserInfoResult> userInfo;

        public ObservableCollection<UserInfoResult> UserInfo
        {
            get { return userInfo; }
            set
            {
                userInfo = value;
                if (userInfo == null)
                {
                    userInfo = new ObservableCollection<UserInfoResult>();
                }
                userInfo.Insert(0, new UserInfoResult() { FullName = "无导购员", UserCode = "0" });

                foreach (var item in value)
                {
                    item.parent = this;
                }
                this.OnPropertyChanged(o => o.UserInfo);
            }
        }
        /// <summary>
        /// 导购员工号
        /// </summary>
        private string userCode;

        public string UserCode
        {
            get { return userCode; }
            set { userCode = value; this.OnPropertyChanged(o => o.UserInfo); }
        }



        /// <summary>
        /// 设置导购员
        /// </summary>
        public ICommand ComfirmCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    if (string.IsNullOrEmpty(UserCode))
                    {
                        UserCode = "0";
                    }
                    SetSaleManParams _params = new SetSaleManParams()
                    {
                        StoreId = _machineInfo.StoreId,
                        MachineSn = _machineInfo.MachineSn,
                        CID = _machineInfo.CompanyId,
                        SaleMan = UserCode,
                        Source = pageSource,
                        Mode =pageSource

                    };
                    var result = ApiManager.Post<SetSaleManParams, ApiRetrunResult<SaleManInfo>>(@"api/SetSaleMan", _params);
                    if (result.Code == "200")
                    {
                        //UserCode = result.Result.SaleManCode;
                        if (string.IsNullOrEmpty(result.Result.SaleManCode))
                        {
                            switch (pageSource)
                            {
                                case 0:
                                default:
                                    PosViewModel.Current.SaleMan = string.Empty;
                                    PosViewModel.Current.SalemanName = string.Empty;
                                    break;
                                case 1:
                                    RefundChangeViewModel.Current.Change.OldSaleMan = string.Empty;
                                    break;
                                case 2:
                                    RefundChangeViewModel.Current.Refund.OldSaleMan = string.Empty;
                                    break;
                            }
                        }
                        else
                        {
                            switch (pageSource)
                            {
                                case 0:
                                default:
                                    PosViewModel.Current.SaleMan = string.Format("导购员 [{0}]{1}", result.Result.SaleManCode, result.Result.SaleManName);
                                    PosViewModel.Current.SalemanName = result.Result.SaleManName;
                                    break;
                                case 1:
                                    RefundChangeViewModel.Current.Change.OldSaleMan = result.Result.SaleManCode;
                                    break;
                                case 2:
                                    RefundChangeViewModel.Current.Refund.OldSaleMan = result.Result.SaleManCode;
                                    break;
                            }

                        }
                        CurrentWindow.Close();
                    }
                    else
                    {
                        Toast.ShowMessage(result.Message, CurrentWindow);
                        UserCode = string.Empty;
                    }
                });
            }
        }

    }
}
