using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.ObjectModels.DTOs
{
    /// <summary>
    /// 促销动作
    /// </summary>
    public class MarketingAction
    {
        /// <summary>
        /// 促销动作类型
        /// </summary>
        public MarketingActionMode MarketingActionMode { get; set; }

        /// <summary>
        /// 赠品列表（Key可为赠品条码，或者加购赠品列表的条码，Value 可为赠品数量)
        /// </summary>
        public IEnumerable<KeyValuePair<string, decimal>> Gifts { get; set; }

        /// <summary>
        /// 活动促销优惠数量(0、无限制)
        /// </summary>
        public decimal MarketingActionNumber { get; set; }

        /// <summary>
        /// 加钱
        /// </summary>
        public decimal AddMoney { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// 折后金额
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// 是否允许累加促销
        /// </summary>
        public bool Repeatable { get; set; }

        /// <summary>
        /// 代金券金额 或者 返现金额
        /// </summary>
        public decimal Amount { get; set; }


    }
}
