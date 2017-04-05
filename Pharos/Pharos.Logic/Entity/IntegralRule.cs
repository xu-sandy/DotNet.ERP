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
    /// 用于管理本系统的积分规则信息 
    /// </summary>
    [Serializable]
    public class IntegralRule
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
        /// 状态（0：生效；1：已无效）
        /// [长度：10]
        /// </summary>
        public int State { get; set; }

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
        /// 适用客户群ID(-1:全部)
        /// [长度：-1]
        /// [不允许为空]
        /// [默认值：((-1))]
        /// </summary>
        public string UseUsers { get; set; }

        /// <summary>
        /// 满足条件金额 
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// </summary>
        public decimal Condition { get; set; }

        /// <summary>
        /// 赠送多少积分 
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int ReturnValue { get; set; }

        /// <summary>
        /// 规则方式(1:按消费金额；2：按付款方式（支付宝；微信；即付宝）； 3：按指定品类；4：按具体商品；5：按消费次数) 
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short RuleTypeId { get; set; }

        /// <summary>
        /// 指定规则方式
        /// [长度：5]
        /// </summary>
        public string RuleTypeValue { get; set; }
        /// <summary>
        /// 是否叠加（0：否；1：是）
        /// </summary>
        public bool IsStack { get; set; }
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
        /// <summary>
        /// 说明
        /// </summary>
        public string Memo { get; set; }
    }
}
