using Newtonsoft.Json;
using Pharos.Logic.OMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Models
{
    /// <summary>
    /// Qct 商户支付交易付款公共响应参数
    /// </summary>
    [Serializable]
    public class PayTradeResponse : BaseTradeResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public PayTradeResponse()
        {

        }
        /// <summary>
        /// 构造商户返回基本信息
        /// </summary>
        /// <param name="order"></param>
        public PayTradeResponse(TradeOrder order) : base(order) { }

        /// <summary>
        /// 商户支付订单号，与请参数中数据一致，原样返回
        /// </summary>
        [JsonProperty("out_trade_no")]
        public string Out_Trade_No { get; set; }
    }
}
