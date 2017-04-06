using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 设定商品
    /// </summary>
    public class PrivilegeProduct
    {
        public int Id { get; set; }
        /// <summary>
        /// 条码或类别
        /// </summary>
        public string BarcodeOrCategorySN { get; set; }
        /// <summary>
        /// 类型(1-商品条码2-商品类型)
        /// </summary>
        public short Type { get; set; }
        public int PrivilegeSolutionId { get; set; }
        public virtual PrivilegeSolution Solution { get; set; }
        /// <summary>
        /// 设置范围值
        /// </summary>
        public virtual List<PrivilegeRegionVal> RegionVals { get; set; }
    }
}
