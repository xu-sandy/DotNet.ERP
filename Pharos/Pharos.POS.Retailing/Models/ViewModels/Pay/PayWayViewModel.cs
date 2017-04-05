﻿using Pharos.POS.Retailing.ChildWin;
using Pharos.POS.Retailing.Models.ApiParams;
using Pharos.POS.Retailing.Models.ApiReturnResults;
using Pharos.POS.Retailing.Models.PosModels;
using Pharos.Wpf.ViewModelHelpers;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using Pharos.POS.Retailing.ChildWin.Pay;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class PayWayViewModel : BaseViewModel
    {
        public PayWayViewModel(decimal amount, PayAction mode, int reason = 0)
        {
            IsOperatEnabled = true;
            Amount = amount;
            Mode = mode;
            this.ViewModelBinded += PayWayViewModel_ViewModelBinded;
            Reason = reason;
            ShowMiltiPay = Visibility.Visible;
            if (Amount <= 0)
                ShowMiltiPay = Visibility.Hidden;
            this.OnPropertyChanged(o => o.ShowMiltiPay);
            var list = new List<char>();
            for (var i = 65; i <= 90; i++)
            {
                list.Add((char)i);
            }
            keys = list.ToArray();
        }

        void PayWayViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsOperatEnabled")
            {
                IsOperatEnabled = !Payways.Any(o => o.IsOperatEnabled);
                this.OnPropertyChanged(o => o.IsOperatEnabled);
            }
        }
        int Reason { get; set; }
        void PayWayViewModel_ViewModelBinded(BaseViewModel model)
        {
            Payways = Global.Payways;
            Global.PaywaysRefreshEvent += Global_PaywaysRefreshEvent;
        }

        void Global_PaywaysRefreshEvent(object obj)
        {
            Payways = Global.Payways;
        }
        public decimal Amount { get; set; }

        private string saleMan;

        public string SaleMan
        {
            get { return saleMan; }
            set
            {
                saleMan = value;
                var _machinesInfo = Global.MachineSettings.MachineInformations;
                SetSaleManParams _params = new SetSaleManParams()
                {
                    StoreId = _machinesInfo.StoreId,
                    MachineSn = _machinesInfo.MachineSn,
                    CID = _machinesInfo.CompanyId,
                    SaleMan = saleMan
                };
                var result = ApiManager.Post<SetSaleManParams, ApiRetrunResult<string>>(@"api/SetSaleMan", _params);
                if (result.Code == "200")
                {
                    saleMan = result.Result;
                }
                else
                {

                    Toast.ShowMessage(result.Message, CurrentWindow);
                    saleMan = string.Empty;
                }
                this.OnPropertyChanged(o => o.SaleMan);
            }
        }

        public Visibility ShowMiltiPay { get; set; }
        public PayAction Mode { get; set; }
        /// <summary>
        /// 多方式支付事件
        /// </summary>
        public ICommand MultiPayCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    MultiPay page = new MultiPay(Amount, Mode);
                    page.Owner = Application.Current.MainWindow;
                    CurrentWindow.Hide();
                    page.ShowDialog();
                    CurrentWindow.Close();
                });
            }
        }
        /// <summary>
        /// 关闭按钮事件
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    CurrentWindow.Close();
                });
            }
        }
        internal char[] keys;
        IEnumerable<PayItem> _Payways;
        public IEnumerable<PayItem> Payways
        {
            get { return _Payways; }
            set
            {
                _Payways = value;
                var i = 0;
                foreach (var item in _Payways)
                {
                    if (i < 36)
                        item.Key = keys[i];
                    item.Amount = Amount;
                    item.Action = Mode;
                    item.CurrentWindow = CurrentWindow;
                    item.Reason = Reason;
                    item.PropertyChanged += PayWayViewModel_PropertyChanged;
                    i++;
                }
                this.OnPropertyChanged(o => o.Payways);
            }
        }

        bool _IsOperatEnabled = true;

        public bool IsOperatEnabled
        {
            get { return _IsOperatEnabled; }
            set
            {
                _IsOperatEnabled = value;
                this.OnPropertyChanged(o => o.IsOperatEnabled);
            }
        }
    }
}
