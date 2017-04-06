using Newtonsoft.Json;
using Pharos.Logic.OMS.Entity;
using QCT.Pay.Common;

namespace Pharos.Logic.OMS.Models
{
    /// <summary>
    /// 商家收款扫码响应参数Model，（对应融合支付：被扫支付）
    /// </summary>
    public class PayMerchScanResponse : PayTradeResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public PayMerchScanResponse()
        {

        }
        /// <summary>
        /// 根据参数构造返回结果
        /// </summary>
        /// <param name="order"></param>
        /// <param name="rsp"></param>
        public PayMerchScanResponse(TradeOrder order, SxfScanPayResponse rsp)
            : base(order)
        {
            Return_Code = PayTradeHelper.TransCodeBySxf(rsp.RspCod);
            Return_Msg = rsp.RspMsg;
            Sign_Type = PayConst.DEF_SIGNTYPE;
            Version = PayConst.DEF_VERSION;

            Out_Trade_No = rsp.PayOrderNo;
            Receipt_Amount = PayTradeHelper.FromCent2Yuan(rsp.TxAmt);
            Pay_Status = (PayTradeHelper.Convert2EnumString<PayState>(PayTradeHelper.Convert2EnumValue<SxfPayState>(rsp.PayResult))).ToUpper();
            Pay_Channel = rsp.PayChannel;
        }
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
    }
}
