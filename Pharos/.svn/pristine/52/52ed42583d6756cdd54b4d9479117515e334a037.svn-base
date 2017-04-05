using Newtonsoft.Json;
using Pharos.Logic.OMS.Entity;
using System;

namespace Pharos.Logic.OMS.Models
{
    /// <summary>
    /// Qct 商户支付交易公共响应参数
    /// </summary>
    [Serializable]
    public class BaseTradeResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public BaseTradeResponse() { }
        /// <summary>
        /// 构造商户返回基本信息
        /// </summary>
        /// <param name="order"></param>
        public BaseTradeResponse(TradeOrder order)
        {
            Mch_Id = order.CID;
            Store_Id = order.SID;
            Device_Id = order.DeviceId;
        }

        /// <summary>
        /// 返回码，00000表示成功，其他均为错误
        /// </summary>
        [JsonProperty("return_code")]
        public string Return_Code { get; set; }
        /// <summary>
        /// 返回信息，SUCCESS表示成功，其他均为错误信息
        /// </summary>
        [JsonProperty("return_msg")]
        public string Return_Msg { get; set; }
        /// <summary>
        /// 商户编号
        /// </summary>
        [JsonProperty("mch_id")]
        public int Mch_Id { get; set; }
        /// <summary>
        /// 门店编号
        /// </summary>
        [JsonProperty("store_id")]
        public string Store_Id { get; set; }
        /// <summary>
        /// 终端编号
        /// </summary>
        [JsonProperty("device_id")]
        public string Device_Id { get; set; }
        /// <summary>
        /// 签名方式
        /// </summary>
        [JsonProperty("sign_type")]
        public string Sign_Type { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }
        /// <summary>
        /// 签名数据
        /// </summary>
        [JsonProperty("sign")]
        public string Sign { get; set; }
    }
}
