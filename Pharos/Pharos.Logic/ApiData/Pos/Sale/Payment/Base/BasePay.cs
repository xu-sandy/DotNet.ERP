﻿using Pharos.Logic.ApiData.Pos.DataAdapter;
using Pharos.Logic.ApiData.Pos.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Pharos.Logic.ApiData.Pos.Sale.Payment
{
    public abstract class BasePay : IPay
    {
        internal BasePay() { }
        public BasePay(int apiCode, PayMode mode)
        {
            ApiCode = apiCode;
            ApiCodes = apiCode.ToString();
            Mode = mode;
        }
        /// <summary>
        /// 初始化支付基础信息
        /// </summary>
        /// <param name="companyId"></param>
        public virtual void Init(int companyId, string storeId, string machineSn, string deviceSn)
        {
            //init
            CompanyId = companyId;
            StoreId = storeId;
            MachineSn = machineSn;
            DeviceSn = deviceSn;

            //find db settings
            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, StoreId, MachineSn, CompanyId, DeviceSn);
            var settings = dataAdapter.GetApiSettings(ApiCode);
            if (settings == null)
                throw new PosException(string.Format("支付接口编号{0}，尚未配置！", ApiCodes));
            ApiUrl = settings.ApiUrl;
            Title = settings.Title;
            Enable = Convert.ToBoolean(settings.State);
            EnableIcon = settings.ApiIcon;
            DisableIcon = settings.ApiCloseIcon;
            OrderNumber = settings.ApiOrder;
        }
        [JsonIgnore]
        public int OrderNumber { get; set; }

        [JsonIgnore]
        public int CompanyId { get; set; }
        [JsonIgnore]
        public string StoreId { get; set; }
        [JsonIgnore]
        public string MachineSn { get; set; }
        [JsonIgnore]
        public string DeviceSn { get; set; }

        [JsonIgnore]
        public Action<IPay> CallBack { get; set; }
        public string Title { get; set; }

        [JsonIgnore]
        public int ApiCode { get; set; }

        [JsonIgnore]
        public string ApiUrl { get; set; }

        public bool Enable { get; set; }

        public string EnableIcon { get; set; }
        public string DisableIcon { get; set; }

        public PayMode Mode { get; set; }


        [JsonIgnore]
        public PayDetails PayDetails { get; set; }
        /// <summary>
        /// 完成支付时，保存支付信息
        /// </summary>
        public virtual void SavePayInfomactions()
        {
            if (PayDetails == null || string.IsNullOrEmpty(StoreId) || string.IsNullOrEmpty(MachineSn))
            {
                throw new PosException("支付信息不全，无法支付！");
            }
            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, StoreId, MachineSn, CompanyId, DataAdapterFactory.DEFUALT);
            dataAdapter.RecordPayment(PayDetails.PaySn, PayDetails.Amount, ApiCode, PayDetails.Change, PayDetails.Receive, PayDetails.ApiOrderSn, PayDetails.CardNo);
            //保存抹零数据
            if (PayDetails.WipeZero != 0)
            {
                dataAdapter.AddWipeZero(CompanyId, PayDetails.PaySn, PayDetails.WipeZero);
            }

            if (CallBack != null)
            {
                CallBack(this);
            }
        }

        /// <summary>
        /// 刷新支付可用状态
        /// </summary>
        /// <param name="storeId">门店</param>
        /// <param name="companyToken">CID</param>
        public virtual void RefreshStatus()
        {
            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, StoreId, MachineSn, CompanyId, DeviceSn);
            var settings = dataAdapter.GetApiSettings(ApiCode);
            ApiUrl = settings.ApiUrl;
            Title = settings.Title;
            Enable = Convert.ToBoolean(settings.State);
            EnableIcon = settings.ApiIcon;
            DisableIcon = settings.ApiCloseIcon;
        }


        public string ApiCodes { get; set; }
    }
}
