using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Models
{
    /// <summary>
    /// 支付查询响应数据基础Model
    /// </summary>
    [Serializable]
    public class BasePayQueryResponse
    {
        /// <summary>
        /// 商户支付订单号
        /// </summary>
        [JsonProperty("out_trade_no")]
        public string Out_Trade_No { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        [JsonProperty("order_type")]
        public string Order_Type { get; set; }
        /// <summary>
        /// 购买者手机号
        /// </summary>
        [JsonProperty("buyer_mobile")]
        public string Buyer_Mobile { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        [JsonProperty("total_amount")]
        public decimal Total_Amount { get; set; }
        /// <summary>
        /// 支付订单状态，支付订单状态：U：未支付； S：支付成功；F：支付失败； C：已撤销；T：支付超时
        /// </summary>
        [JsonProperty("pay_status")]
        public string Pay_Status { get; set; }
        /// <summary>
        /// 支付渠道，WX：微信支付，ALIPAY：支付宝支付
        /// </summary>
        [JsonProperty("pay_channel")]
        public string Pay_Channel { get; set; }
        /// <summary>
        /// 支付日期，支付完成日期，格式yyyy-MM-dd:HH mm ss
        /// </summary>
        [JsonProperty("pay_date")]
        public string Pay_Date { get; set; }
    }
}
