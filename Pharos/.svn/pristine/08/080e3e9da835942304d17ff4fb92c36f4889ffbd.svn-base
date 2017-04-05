using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Pharos.Logic.OMS.Models
{
    /// <summary>
    /// 退款申请请求
    /// </summary>
    public class RefundApplyRequest : BaseTradeRequest
    {
        #region Methods
        /// <summary>
        /// 重置RfdNotifyUrl
        /// </summary>
        /// <param name="url"></param>
        public void ResetRfdNotifyUrl(string url)
        {
            if (string.IsNullOrEmpty(this.Refund_Notify_Url))
            {
                this.Refund_Notify_Url = url;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// 商户退款订单号，每笔退款订单的唯一标识，商户需保持该字段在系统内唯一
        /// </summary>
        [Required(ErrorMessage = "out_refund_no退款订单号不可为空")]
        [JsonProperty("out_refund_no")]
        public string Out_Refund_No { get; set; }
        /// <summary>
        /// 原商户支付订单号
        /// </summary>
        [Required(ErrorMessage = "out_trade_no原支付订单号不可为空")]
        [JsonProperty("out_trade_no")]
        public string Out_Trade_No { get; set; }

        /// <summary>
        /// 退款金额，以元为单位
        /// </summary>
        [Required(ErrorMessage = "refund_amount退款金额不可为空")]
        [JsonProperty("refund_amount")]
        [Range(0.02, 9999999.99, ErrorMessage = "refund_amount 金额超出范围，最小金额为0.02元")]
        public decimal Refund_Amount { get; set; }
        /// <summary>
        /// 退款结果通知地址，如果该值为空，则使用商户开户时默认设置的退款结果通知地址
        /// </summary>
        [JsonProperty("refund_notify_url")]
        public string Refund_Notify_Url { get; set; }
        /// <summary>
        /// 退款原由
        /// </summary>
        [JsonProperty("refund_reason")]
        public string Refund_Reason { get; set; }
        #endregion
    }
}
