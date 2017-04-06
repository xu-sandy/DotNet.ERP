using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pharos.Logic.OMS.Models
{
    /// <summary>
    /// 门店静态扫码支付
    /// </summary>
    [Serializable]
    public class BaseMerchStoreModel
    {
        /// <summary>
        /// 商户号
        /// </summary>
        [Required(ErrorMessage = "mch_id 字段必输")]
        [JsonProperty("mch_id")]
        public int Mch_Id { get; set; }
        /// <summary>
        /// 门店号
        /// </summary>
        [Required(ErrorMessage = "store_id 字段必输")]
        [JsonProperty("store_id")]
        public string Store_Id { get; set; }
        /// <summary>
        /// 商户简称
        /// </summary>
        [JsonProperty("mch_title")]
        public string Mch_Title { get; set; }
        /// <summary>
        /// 商户门店名称
        /// </summary>
        [JsonProperty("store_name")]
        public string Store_Name { get; set; }
    }
}
