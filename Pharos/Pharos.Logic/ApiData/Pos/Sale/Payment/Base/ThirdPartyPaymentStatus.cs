using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Sale.Payment
{
    public enum ThirdPartyPaymentStatus
    {
        /// <summary>
        /// 未知状态或等待支付
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 失败
        /// </summary>
        Error = -1,
        /// <summary>
        /// 完成
        /// </summary>
        Complete = 1
    }
}
