using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pharos.Logic.OMS.Models
{
    /// <summary>
    /// 商家收款扫码请求参数Model，（对应融合支付：被扫支付）
    /// </summary>
    [Serializable]
    public class PayMerchScanRequest : PayTradeRequest
    {
        /// <summary>
        /// 购买者的支付二维码字符串
        /// </summary>
        [Required(ErrorMessage = "buyer_pay_token字段是必需的")]
        [MaxLength(128, ErrorMessage = "buyer_pay_token字段超出最大长度")]
        [JsonProperty("buyer_pay_token")]
        public string Buyer_Pay_Token { get; set; }
    }
}
