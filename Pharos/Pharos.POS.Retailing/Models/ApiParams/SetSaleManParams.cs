﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.POS.Retailing.Models.ApiParams
{
    public class SetSaleManParams : BaseApiParams
    {
        /// <summary>
        /// 导购员
        /// </summary>
        public string SaleMan { get; set; }
        public int Mode { get; set; }
        public int Source { get; set; }
    }
}
