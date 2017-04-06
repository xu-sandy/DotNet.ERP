using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Models
{
    /// <summary>
    /// 单笔支付订单查询响应参数
    /// </summary>
    [Serializable]
    public class PayBatchQueryResponse : BaseTradeResponse
    {
        /// <summary>
        /// 查询起始日期
        /// </summary>
        [JsonProperty("start_date")]
        public string Start_Date { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
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
        [JsonProperty("page_num")]
        public int Page_Num { get; set; }
        /// <summary>
        /// 每页记录数
        /// </summary>
        [JsonProperty("page_size")]
        public int Page_Size { get; set; }
        /// <summary>
        /// 商户批量查询订单List
        /// </summary>
        [JsonProperty("order_list")]
        public List<BasePayQueryResponse> Order_List { get; set; }
    }
}
