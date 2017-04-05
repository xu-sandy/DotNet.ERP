using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity
{
    public class SalesRecord : BaseEntity
    {
        public string MarketingId { get; set; }
        /// <summary>
        /// 商店Id
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 剩余销售数
        /// </summary>
        public decimal Number { get; set; }
        /// <summary>
        /// 日期 （每日限购不能为空）
        /// </summary>
        public DateTime CreateDT { get; set; }
    }
}
