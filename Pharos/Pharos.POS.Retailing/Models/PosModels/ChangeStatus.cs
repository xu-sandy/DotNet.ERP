﻿using Pharos.Wpf.Models;

namespace Pharos.POS.Retailing.Models.PosModels
{
    /// <summary>
    /// 退换货状态枚举
    /// </summary>
    public enum ChangeStatus
    {
        [EnumTitle(2, "退货")]
        Refund = 2,
        [EnumTitle(1, "换货")]
        Change = 1,
        /// <summary>
        /// 退单
        /// </summary>
        RefundAll=3
    }
}
