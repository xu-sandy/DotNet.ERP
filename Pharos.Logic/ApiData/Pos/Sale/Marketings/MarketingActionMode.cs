using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Sale.Marketings
{
    public enum MarketingActionMode
    {
        /// <summary>
        /// 马上返回现金，将影响应收金额
        /// </summary>
        NowCash,
        /// <summary>
        /// 结账后返回现金，将不影响应收金额
        /// </summary>
        AfterCash,
        /// <summary>
        /// 代金券
        /// </summary>
        CashCoupon,
        /// <summary>
        /// 赠品
        /// </summary>
        Gift,
        /// <summary>
        /// 加钱赠送
        /// </summary>
        AddMoneyToGive,
        /// <summary>
        /// 折扣
        /// </summary>
        Discount,
        /// <summary>
        /// 折后金额
        /// </summary>
        DiscountAmount,
        /// <summary>
        /// 多份折后金额
        /// </summary>
        RepetitionDiscountAmount,

    }
}
