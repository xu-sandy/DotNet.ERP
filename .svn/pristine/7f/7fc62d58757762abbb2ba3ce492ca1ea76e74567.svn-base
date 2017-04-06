using Pharos.Logic.OMS.Models;
using QCT.Pay.Common;
using QCT.Pay.Common.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Entity
{
    /// <summary>
    /// 交易流水扩展实体
    /// </summary>
    [NotMapped]
    [Serializable]
    public partial class TradeOrderExt : TradeOrder
    {
        /// <summary>
        /// 收单渠道
        /// </summary>
        public string ChannelCode { get; set; }
        /// <summary>
        /// 交易时间
        /// </summary>
        public string TradeDateTime { get; set; }
    }
    public partial class TradeOrder
    {
        public TradeOrder(BaseTradeRequest model, MerchantStoreChannelModel merchStore)
        {
            DateTime createDt = DateTime.Now;
            CID = model.Mch_Id;
            SID = model.Store_Id;
            DeviceId = model.Device_Id;
            DeviceId3 = model.Device_Id.ToString();
            SignType = model.Sign_Type;
            Version = model.Version;
            Signature = model.Sign;
            MchId3 = merchStore.MerchId3;
            StoreId3 = merchStore.StoreId3;
            CreateDT = createDt;
            UpdateDT = createDt;
            ChannelNo = merchStore.ChannelNo;
        }
        public TradeOrder(PayTradeRequest model, MerchantStoreChannelModel merchStore,string tradeNo)
            : this((BaseTradeRequest)model, merchStore)
        {
            model.ResetPayNotifyUrl(merchStore.PayNotifyUrl);
            TradeNo = tradeNo;
            OutTradeNo = model.Out_Trade_No;
            TotalAmount = model.Total_Amount;
            SourceType = merchStore.SourceType;
            ApiNo = merchStore.ApiNo;
            State = (short)PayState.NotPay;
            PayNotifyUrl = model.Pay_Notify_Url;
            //Fee = PayRules.CalcFee(model.TotalAmount, merchStore);
            //OrderType3 = short.Parse(model.OrderType3);
            TradeType = (short)QctTradeType.Income;
            FeeType = (short)PayFeeType.RMB;
            BuyerMobile = "";
            GoodsName = string.IsNullOrWhiteSpace(model.Goods_Name) ? "购物消费" : model.Goods_Name;
            GoodsDesc = model.Goods_Desc;
        }
    }
}
