using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Models
{
    /// <summary>
    /// 收单渠道细目
    /// </summary>
    [Serializable]
    public class PayChannelModel
    {
        /// <summary>
        /// 商户支付调用接口编号
        /// </summary>
        public int ApiNo { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public short OptType { get; set; }
        /// <summary>
        /// 渠道支付方式
        /// </summary>
        public short ChannelPayMode { get; set; }
        /// <summary>
        /// 单月免费交易额度（元）
        /// </summary>
        public decimal MonthFreeTradeAmount { get; set; }
        /// <summary>
        /// 超出金额服务费率（%）
        /// </summary>
        public decimal OverServiceRate { get; set; }
        /// <summary>
        /// 单笔服务费上限（元）
        /// </summary>
        public decimal SingleServFeeUpLimit { get; set; }
        /// <summary>
        /// 单笔服务费下限（元）
        /// </summary>
        public decimal SingleServFeeLowLimit { get; set; }
    }
}
