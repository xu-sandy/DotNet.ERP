using Pharos.Logic.ApiData.Pos.DataAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Sale.Payment
{
    public class CashPay : BasePay
    {
        public CashPay()
            : base(11, PayMode.CashPay)
        {
        }
    }
}
