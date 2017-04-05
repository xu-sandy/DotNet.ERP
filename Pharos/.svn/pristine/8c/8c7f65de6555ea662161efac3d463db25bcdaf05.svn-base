using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Models
{
    /// <summary>
    /// 支付订单分页查询请求参数
    /// </summary>
    [Serializable]
    public class PayBatchQueryRequest : BaseTradeRequest
    {
        /// <summary>
        /// 查询起始日期，格式yyyyMMdd
        /// </summary>
        [Required(ErrorMessage = "start_date字段是必需的")]
        [JsonProperty("start_date")]
        public string Start_Date { get; set; }
        /// <summary>
        /// 截止日期，格式yyyyMMdd
        /// </summary>
        [Required(ErrorMessage = "end_date字段是必需的")]
        [JsonProperty("end_date")]
        public string End_Date { get; set; }
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
        /// 页序号
        /// </summary>
        [Required(ErrorMessage = "page_num字段是必需的")]
        [JsonProperty("page_num")]
        public int Page_Num { get; set; }
        /// <summary>
        /// 每页记录数
        /// </summary>
        [Required(ErrorMessage = "page_size字段是必需的")]
        [JsonProperty("page_size")]
        public int Page_Size { get; set; }
    }
}
