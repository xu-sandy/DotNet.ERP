using Newtonsoft.Json;
using System;

namespace Pharos.Logic.OMS.Models
{
    /// <summary>
    /// 单笔退款订单查询响应参数
    /// </summary>
    [Serializable]
    public class RefundQueryResponse : BaseTradeResponse
    {
        /// <summary>
        /// 商户支付订单号
        /// </summary>
        [JsonProperty("refund_trade_no")]
        public string Refund_Trade_No { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        [JsonProperty("refund_amount")]
        public decimal Refund_Amount { get; set; }
        /// <summary>
        /// 支付订单状态，支付订单状态：U：未支付； S：支付成功；F：支付失败； C：已撤销；T：支付超时
        /// </summary>
        [JsonProperty("refund_status")]
        public string Refund_Status { get; set; }
        /// <summary>
        /// 退款日期，支付完成日期，格式yyyy-MM-dd:HH mm ss
        /// </summary>
        [JsonProperty("refund_date")]
        public string Refund_Date { get; set; }
    }
}
