using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Sale.Marketings
{
    /// <summary>
    /// 配额模式
    /// </summary>
    public enum MarketingQuotaMode
    {
        /// <summary>
        /// 总配额
        /// </summary>
        TotalQuota = 2,
        /// <summary>
        /// 按天配额
        /// </summary>
        EveryDay = 1,
        /// <summary>
        /// 无限制
        /// </summary>
        NotLimit = 0
    }
}
