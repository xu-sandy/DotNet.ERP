using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.ValueObject
{
    public class SaleStatistics
    {
        /// <summary>
        /// 商品优惠前合计
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// 商品优惠合计
        /// </summary>
        public decimal Preferential { get; set; }
        /// <summary>
        /// 满减
        /// </summary>
        public decimal ManJianPreferential { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal Receivable { get; set; }

        /// <summary>
        /// 商品件数
        /// </summary>
        public int Num { get; set; }

        public string OrderSn { get; set; }
        /// <summary>
        /// 满件立减金额
        /// </summary>
        public decimal ZuHePreferential { get; set; }
        /// <summary>
        /// 满元立减金额
        /// </summary>
        public decimal ManYuanPreferential { get; set; }
    }
}
