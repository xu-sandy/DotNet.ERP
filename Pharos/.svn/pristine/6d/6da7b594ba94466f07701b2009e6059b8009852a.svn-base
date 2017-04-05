using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 设定商品值
    /// </summary>
    public class PrivilegeRegionVal
    {
        public int Id { get; set; }
        /// <summary>
        /// 设定值
        /// </summary>
        public decimal? Value { get; set; }
        public int PrivilegeProductId { get; set; }
        public int PrivilegeRegionId { get; set; }

        public virtual PrivilegeRegion Region { get; set; }
        public virtual PrivilegeProduct Product { get; set; }

        /// <summary>
        ///设置赠品
        /// </summary>
        public virtual List<PrivilegeProductGift> ProductGifts { get; set; }

    }
}
