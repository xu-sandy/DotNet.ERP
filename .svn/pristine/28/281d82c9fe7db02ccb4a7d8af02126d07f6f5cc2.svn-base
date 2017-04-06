using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.ObjectModels.DTOs
{
    /// <summary>
    /// 促销活动限制规则
    /// </summary>
    public class MarketingRule
    {

        public string Id { get; set; }

        /// <summary>
        /// 促销名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 促销类型
        /// </summary>
        public MarketingType Type { get; set; }

        /// <summary>
        /// 是否已过时
        /// </summary>
        public bool IsTimeOut { get; set; }

        /// <summary>
        /// 促销是否可用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 创建规则日期
        /// </summary>
        public DateTime CreateRuleDate { get; set; }

        /// <summary>
        /// 促销活动规则计量模式
        /// </summary>
        public MeteringMode MeteringMode { get; set; }
        /// <summary>
        /// 条码范围
        /// </summary>
        public IEnumerable<string> BarcodeRange { get; set; }

        /// <summary>
        /// 不参与条码
        /// </summary>
        public IEnumerable<string> IgnoreBarcodeRange { get; set; }
        /// <summary>
        /// 规则数量
        /// </summary>
        public decimal RuleNumber { get; set; }
        /// <summary>
        /// 限购量（次数） 
        /// </summary>
        public decimal RestrictionBuyCount { get; set; }
        /// <summary>
        /// 配额模式
        /// </summary>
        public MarketingQuotaMode MarketingQuotaMode { get; set; }

        /// <summary>
        /// 客户类型
        /// </summary>
        public CustomerType CustomerType { get; set; }

        /// <summary>
        /// 促销活动
        /// </summary>
        public MarketingAction MarketingAction { get; set; }

        /// <summary>
        /// 是否允许累加促销
        /// </summary>
        public bool IsRepeatMarketing { get; set; }
    }
}
