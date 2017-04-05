﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.ValueObject
{
    public class BillDetails
    {
        public string Barcode { get; set; }

        public string ProductCode { get; set; }
        public string Title { get; set; }

        public string Unit { get; set; }

        public decimal Number { get; set; }

        public decimal SysPrice { get; set; }

        public decimal ActualPrice { get; set; }

        public string Size { get; set; }

        public string Brand { get; set; }

        public decimal Total { get; set; }
        /// <summary>
        /// 商品明细销售状态
        /// </summary>
        public int SalesClassifyId { get; set; }

    }
}
