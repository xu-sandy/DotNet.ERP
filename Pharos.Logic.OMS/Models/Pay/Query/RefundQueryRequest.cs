using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pharos.Logic.OMS.Models
{
    /// <summary>
    /// 单笔退款订单查询请求参数
    /// </summary>
    [Serializable]
    public class RefundQueryRequest : BaseTradeRequest
    {
        /// <summary>
        /// 商户支付订单号
        /// </summary>
        [Required(ErrorMessage = "refund_trade_no字段是必需的")]
        [JsonProperty("refund_trade_no")]
        public string Refund_Trade_No { get; set; }
    }
}
