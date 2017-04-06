using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pharos.Logic.OMS.Models
{
    /// <summary>
    /// 单笔支付订单查询请求参数
    /// </summary>
    [Serializable]
    public class PayQueryRequest : BaseTradeRequest
    {
        /// <summary>
        /// 商户支付订单号，与请参数中数据一致，原样返回
        /// </summary>
        [Required(ErrorMessage = "out_trade_no字段是必需的")]
        [JsonProperty("out_trade_no")]
        public string Out_Trade_No { get; set; }
    }
}
