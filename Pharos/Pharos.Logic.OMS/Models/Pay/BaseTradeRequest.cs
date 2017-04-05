using Newtonsoft.Json;
using QCT.Pay.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pharos.Logic.OMS.Models
{
    /// <summary>
    /// Qct 商户支付交易公共请求参数
    /// </summary>
    [Serializable]
    public class BaseTradeRequest
    {
        #region Properties
        /// <summary>
        /// 请求使用的编码格式，如utf-8,gbk,gb2312等 默认utf-8
        /// </summary>
        [Required(ErrorMessage = "charset字段是必需的")]
        [JsonProperty("charset")]
        public string Charset { get; set; }
        /// <summary>
        /// 接口名称
        /// </summary>
        [Required(ErrorMessage = "method字段是必需的")]
        [JsonProperty("method")]
        public string Method { get; set; }
        /// <summary>
        /// 商户编号，无卡支付平台给接入方分配的唯一标识
        /// </summary>
        [Required(ErrorMessage = "mch_id字段是必需的")]
        [JsonProperty("mch_id")]
        public int Mch_Id { get; set; }
        /// <summary>
        /// 门店编号
        /// </summary>
        [Required(ErrorMessage = "store_id字段是必需的")]
        [JsonProperty("store_id")]
        public string Store_Id { get; set; }
        /// <summary>
        /// 终端编号
        /// </summary>
        [Required(AllowEmptyStrings = true, ErrorMessage = "device_id需为可空的必传参数")]
        [JsonProperty("device_id")]
        public string Device_Id { get; set; }
        /// <summary>
        /// 签名方式，MD5
        /// </summary>
        [Required(ErrorMessage = "sign_type字段是必需的")]
        [JsonProperty("sign_type")]
        public string Sign_Type { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        [Required(ErrorMessage = "version字段是必需的")]
        [JsonProperty("version")]
        public string Version { get; set; }
        /// <summary>
        /// 签名数据
        /// </summary>
        [Required(ErrorMessage = "sign字段是必需的")]
        [JsonProperty("sign")]
        public string Sign { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// 验证Model格式
        /// </summary>
        /// <returns></returns>
        public string TryValidateObject()
        {
            //var result = string.Empty;
            //var context = new ValidationContext(this, null, null);
            //var results = new List<ValidationResult>();
            //var resultState = Validator.TryValidateObject(this, context, results, true);
            //if (!resultState)
            //{
            //    results.ForEach(o =>
            //    {
            //        result += "," + o.ToString();
            //    });
            //    if (result.Length > 0)
            //        result = result.Substring(1);
            //}
            //return result;
            return PayTradeHelper.TryValidateObject(this);
        }
        #endregion
    }
}
