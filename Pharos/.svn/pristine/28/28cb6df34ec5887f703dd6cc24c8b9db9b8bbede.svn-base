using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pharos.Logic.OMS.Models
{
    /// <summary>
    /// Qct 商户支付交易付款公共请求参数
    /// </summary>
    [Serializable]
    public class PayTradeRequest : BaseTradeRequest
    {
        #region Methods
        /// <summary>
        /// 重置PayNotifyUrl
        /// </summary>
        public void ResetPayNotifyUrl(string url)
        {
            if (string.IsNullOrEmpty(this.Pay_Notify_Url))
            {
                this.Pay_Notify_Url = url;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// 商户支付订单号，每笔支付订单的唯一标识，商户需保持该字段在系统内唯一,建议生成规则：时 间 (yyyyMMddHHmmss)+4位序号
        /// </summary>
        [Required(ErrorMessage = "out_trade_no字段是必需的")]
        [JsonProperty("out_trade_no")]
        public string Out_Trade_No { get; set; }
        ///// <summary>
        ///// 订单类型，1：普通订单 2：当面收款订单
        ///// </summary>
        //[JsonProperty("order_type")]
        //public string OrderType3 { get; set; }
        /// <summary>
        /// 订单日期，格式： YYYYMMDD
        /// </summary>
        [Required(ErrorMessage = "create_date字段是必需的")]
        [JsonProperty("create_date")]
        public DateTime Create_Date { get; set; }
        /// <summary>
        /// 交易金额，以元为单位
        /// </summary>
        [Required(ErrorMessage = "total_amount 必需大于0")]
        [Range(0.02, 9999999.99, ErrorMessage = "total_amount 金额超出范围，最小金额为0.02元")]
        [JsonProperty("total_amount")]
        public decimal Total_Amount { get; set; }
        /// <summary>
        /// 支付结果通知地址，支付结果的后台通知地址，如果该值为空，则使用商户开户时默认设置的支付结果通知地址
        /// </summary>
        [JsonProperty("pay_notify_url")]
        public string Pay_Notify_Url { get; set; }
        /// <summary>
        /// 购买者手机号，订单类型为 2:当面收款订单，则该值必输
        /// </summary>
        [JsonProperty("buyer_mobile")]
        public string Buyer_Mobile { get; set; }
        /// <summary>
        /// 商品名称，不输，则默认为“购物消费”
        /// </summary>
        [JsonProperty("goods_name")]
        public string Goods_Name { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        [JsonProperty("goods_desc")]
        public string Goods_Desc { get; set; }
        #endregion
    }
}
