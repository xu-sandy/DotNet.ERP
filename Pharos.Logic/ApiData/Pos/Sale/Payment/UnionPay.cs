using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Sale.Payment
{
    /// <summary>
    /// 银联（台账）
    /// </summary>
    public class UnionPay : BasePay
    {
        public UnionPay()
            : base(12, PayMode.UnionPay)
        {
        }
    }
}
