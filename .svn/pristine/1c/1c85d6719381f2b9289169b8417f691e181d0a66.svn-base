using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Entity.View
{
    /// <summary>
    /// 服务费率设置ViewModel
    /// </summary>
    public class PayChannelDetailViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// 收单渠道详情ID
        /// </summary>
        public string ChannelDetailId { get; set; }
        /// <summary>
        /// 渠道编号(从1开始累加)
        /// </summary>
        public int ChannelNo { get; set; }
        /// <summary>
        /// 状态（枚举：0：未启用；1：启用；）
        /// </summary>
        public short State { get; set; }
        /// <summary>
        /// 操作类型：枚举（0：不限；1：收款；2：退款；3转账；4付款；5：查询；）
        /// </summary>
        public string OptType { get; set; }
        /// <summary>
        /// 支付方式（枚举：1：扫码支付；2：网站支付；3：刷卡支付；）
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
        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime EffectiveDate { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime AuditDT { get; set; }
        /// <summary>
        /// 创建人UID
        /// </summary>
        public string AuditUID { get; set; }
    }
}
