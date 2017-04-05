// --------------------------------------------------
// Copyright (C) 2016 版权所有
// 创 建 人：蔡少发
// 创建时间：2016-08-02
// 描述信息：
// --------------------------------------------------

using System;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 用于管理本系统的充值赠送信息 
    /// </summary>
    [Serializable]
    public class RechargeGifts
    {
        /// <summary>
        /// 记录 ID 
        /// [主键：√]
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 公司CID
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((-1))]
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 公司 CID
        /// [长度：40]
        /// [不允许为空]
        /// [默认值：((-1))]
        /// </summary>
        public string RuleId { get; set; }

        /// <summary>
        /// 活动状态（0：为开始；1：活动中；2：已过期）
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 分期（0:即时；1:分期） 
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public int Stage { get; set; }

        /// <summary>
        /// 赠送项目（0：返现；1：返积分） 
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int GiftProject { get; set; }
        /// <summary>
        /// 赠送类型（0：充值；1：消费）
        /// </summary>
        public short Category { get; set; }
        /// <summary>
        /// 赠送条件
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// </summary>
        public decimal ConditionValue { get; set; }
        /// <summary>
        /// 赠送金额
        /// [长度：19]
        /// [可以为空]
        /// </summary>
        public decimal GiftsValue { get; set; }
        /// <summary>
        /// 限量（-1：不限） 
        /// [长度：18]
        /// [不允许为空]
        /// [默认值：((-1))]
        /// </summary>
        public decimal LimitNumber { get; set; }

        /// <summary>
        /// 总期数 
        /// [长度：10]
        /// </summary>
        public int StageNumber { get; set; }

        /// <summary>
        /// 每期返还 
        /// [长度：19，小数位数：4]
        /// </summary>
        public decimal StageAvg { get; set; }

        /// <summary>
        /// 每期返还时间 
        /// [长度：10]
        /// </summary>
        public string ReturnDT { get; set; }

        /// <summary>
        /// 
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public string ExpiryStart { get; set; }

        /// <summary>
        /// 有效期-截止(空为永久)
        /// [长度：10]
        /// </summary>
        public string ExpiryEnd { get; set; }

        /// <summary>
        /// 活动时效（0：每天； 1：按周几； 2；按时间）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public short GiftAging { get; set; }

        /// <summary>
        /// 活动时效具体时间
        /// [长度：50]
        /// </summary>
        public string GiftAgingValue { get; set; }

        /// <summary>
        /// 开始时间段 1（
        /// [长度：8]
        /// </summary>
        public string StartTime1 { get; set; }

        /// <summary>
        /// 结束时间段 1 
        /// [长度：8]
        /// </summary>
        public string EndTime1 { get; set; }

        /// <summary>
        /// 开始时间段 2
        /// [长度：8]
        /// </summary>
        public string StartTime2 { get; set; }

        /// <summary>
        /// 结束时间段 2
        /// [长度：8]
        /// </summary>
        public string EndTime2 { get; set; }

        /// <summary>
        /// 开始时间段 3 
        /// [长度：8]
        /// </summary>
        public string StartTime3 { get; set; }

        /// <summary>
        /// 结束时间段 3 
        /// [长度：8]
        /// </summary>
        public string EndTime3 { get; set; }

        /// <summary>
        /// 创建时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        public DateTime CreateDT { get; set; }

        /// <summary>
        /// 创建人
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CreateUID { get; set; }
    }
}
