﻿using System.Collections.Generic;

namespace Pharos.POS.Retailing.Models.ApiParams
{
    public class GetProductPriceParams : BaseApiParams
    {
        /// <summary>
        /// 查询范围
        /// </summary>
        public decimal From { get; set; }
        /// <summary>
        /// 查询范围
        /// </summary>
        public decimal To { get; set; }
        /// <summary>
        /// 商品分类 ID 数组
        /// </summary>
        public IEnumerable<int> CategorySns { get; set; }
    }
}