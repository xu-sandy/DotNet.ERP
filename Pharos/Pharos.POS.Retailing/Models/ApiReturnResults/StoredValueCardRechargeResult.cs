﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.POS.Retailing.Models.ApiReturnResults
{
    public class StoredValueCardRechargeResult
    {
        public string Name { get; set; }
        public string StateInfo { get; set; }
        public string CardNo { get; set; }
        public decimal balance { get; set; }
        public decimal Amount { get; set; }
    }
}
