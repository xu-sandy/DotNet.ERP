﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Sale.Payment
{
    public class ZhiFuBao : BasePay
    {
        public ZhiFuBao()
            : base(20, PayMode.AliPay)
        {
        }
    }
}
