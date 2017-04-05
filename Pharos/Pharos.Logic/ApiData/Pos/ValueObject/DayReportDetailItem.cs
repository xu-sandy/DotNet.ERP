﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.ValueObject
{
    public class DayReportDetailItem
    {
        public string Project { get; set; }

        public int Number { get; set; }

        public decimal Amount { get; set; }

        public List<PayWayItem> PayWay { get; set; }
    }
}
