using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pharos.Logic.OMS.Models
{
    /// <summary>
    /// 商户门店支付（静态二维码）Model
    /// </summary>
    [Serializable]
    public class PayBuyerScanStaticRequest : BaseMerchStoreModel
    {
        /// <summary>
        /// 门店编号
        /// </summary>
        [Required(ErrorMessage = "交易金额不能小于0.01元")]
        [Range(0.02, 9999999.99, ErrorMessage = "交易金额不能小于0.01元")]
        [JsonProperty("total_amount")]
        public decimal Total_Amount { get; set; }
        [JsonProperty("goods_desc")]
        public string Goods_Desc { get; set; }
    }
}
