using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Entity
{
    /// <summary>
    /// 收单渠道信息扩展
    /// </summary>
    [NotMapped]
    [Serializable]
    public class PayChannelManageExt : PayChannelManage
    {
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string Creater { get; set; }
        /// <summary>
        /// 审核人名称
        /// </summary>
        public string Auditer { get; set; }
        /// <summary>
        /// 收单渠道详情ID
        /// </summary>
        public string ChannelDetailId { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public short? ChannelPayMode { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public string OptType { get; set; }
        /// <summary>
        /// 单月免费交易额度（元）
        /// </summary>
        public decimal? MonthFreeTradeAmount { get; set; }
        /// <summary>
        /// 超出金额服务费率（%）
        /// </summary>
        public decimal? OverServiceRate { get; set; }
        /// <summary>
        /// 单笔服务费上限（元）
        /// </summary>
        public decimal? SingleServFeeLowLimit { get; set; }
        /// <summary>
        /// 单笔服务费下限（元）
        /// </summary>
        public decimal? SingleServFeeUpLimit { get; set; }
    }
}
