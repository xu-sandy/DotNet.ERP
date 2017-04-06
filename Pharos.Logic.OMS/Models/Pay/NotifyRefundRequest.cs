using Newtonsoft.Json;
using Pharos.Logic.OMS.Entity;
using QCT.Pay.Common;
using System;

namespace Pharos.Logic.OMS.Models
{
    /// <summary>
    /// 退款通知请求Model
    /// </summary>
    [Serializable]
    public class NotifyRefundRequest : BaseTradeRequest
    {
        public NotifyRefundRequest(TradeOrder order, TradeResult result)
        {
            Charset = PayConst.DEF_CHARSET;
            Mch_Id = order.CID;
            Store_Id = order.SID;
            Device_Id = order.DeviceId;
            Sign_Type = PayConst.SXF_DEF_SIGNTYPE;
            Version = PayConst.DEF_VERSION;

            Method = PayConst.QCTTRADE_NOTIFY_REFUND;
            Out_Trade_No = result.OutTradeNo;
            Refund_Amount = result.ReceiptAmount;
            Refund_Status = (PayTradeHelper.Convert2EnumString<RefundState>(result.TradeState)).ToUpper();
            Pay_Channel = PayTradeHelper.Convert2EnumString<PayChannel>(result.PayChannel).ToUpper();
            Refund_Date = PayTradeHelper.Convert2DateFormat(result.TradeDate, result.TradeTime, "yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// 商户退款订单号
        /// </summary>
        [JsonProperty("out_refund_no")]
        public string Out_Trade_No { get; set; }
        /// <summary>
        /// 退款金额，以元为单位
        /// </summary>
        [JsonProperty("refund_amount")]
        public decimal Refund_Amount { get; set; }
        /// <summary>
        /// 退款状态，REFUNDING：退款中;REFUNDSUCCESS：退款成功;REFUNDFAIL：退款失败
        /// </summary>
        [JsonProperty("refund_status")]
        public string Refund_Status { get; set; }
        /// <summary>
        /// 支付渠道，WX：微信支付，ALIPAY：支付宝支付
        /// </summary>
        [JsonProperty("pay_channel")]
        public string Pay_Channel { get; set; }
        /// <summary>
        /// 退款日期，支付完成日期，格式yyyyMMdd
        /// </summary>
        [JsonProperty("refund_date")]
        public string Refund_Date { get; set; }
    }
}
