using Newtonsoft.Json;
using Pharos.Logic.OMS.Entity;
using QCT.Pay.Common;
using System;

namespace Pharos.Logic.OMS.Models
{
    /// <summary>
    /// 支付结果后台通知请求参数Model
    /// </summary>
    [Serializable]
    public class NotifyPayRequest : BaseTradeRequest
    {
        /// <summary>
        /// 根据参数构造请求Model对象
        /// </summary>
        /// <param name="order"></param>
        public NotifyPayRequest(TradeOrder order, TradeResult result)
        {
            Charset = PayConst.DEF_CHARSET;
            Mch_Id = order.CID;
            Store_Id = order.SID;
            Device_Id = order.DeviceId;
            Sign_Type = PayConst.SXF_DEF_SIGNTYPE;
            Version = PayConst.DEF_VERSION;

            Method = PayConst.QCTTRADE_NOTIFY_PAY;
            Out_Trade_No = result.OutTradeNo;
            Receipt_Amount = result.ReceiptAmount;
            Pay_Status = PayTradeHelper.Convert2EnumString<PayState>(result.TradeState).ToUpper();
            Pay_Channel = PayTradeHelper.Convert2EnumString<PayChannel>(result.PayChannel).ToUpper();
            Trade_Date = PayTradeHelper.Convert2DateFormat(result.TradeDate, result.TradeTime, "yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// 商户支付订单号
        /// </summary>
        [JsonProperty("out_trade_no")]
        public string Out_Trade_No { get; set; }
        /// <summary>
        /// 交易金额，以元为单位
        /// </summary>
        [JsonProperty("receipt_amount")]
        public decimal Receipt_Amount { get; set; }
        /// <summary>
        /// 支付结果，NOTPAY=未支付,付款中，PAYSUCCESS=支付成功，PAYFAIL=支付失败，PAYCANCEL=已撤销，PAYTIMEOUT=支付超时；
        /// </summary>
        [JsonProperty("pay_status")]
        public string Pay_Status { get; set; }
        /// <summary>
        /// 支付渠道，WX：微信支付，ALIPAY：支付宝支付
        /// </summary>
        [JsonProperty("pay_channel")]
        public string Pay_Channel { get; set; }
        /// <summary>
        /// 支付日期，支付完成日期，格式yyyyMMdd
        /// </summary>
        [JsonProperty("trade_date")]
        public string Trade_Date { get; set; }
    }
}
