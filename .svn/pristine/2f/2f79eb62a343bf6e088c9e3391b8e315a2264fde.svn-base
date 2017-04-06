using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 设定区间段
    /// </summary>
    public class PrivilegeRegion
    {
        public int Id { get; set; }
        /// <summary>
        /// 开始值
        /// </summary>
        public decimal? StartVal { get; set; }
        /// <summary>
        /// 结束值
        /// </summary>
        public decimal? EndVal { get; set; }
        public int PrivilegeSolutionId { get; set; }
        public virtual PrivilegeSolution Solution { get; set; }

        public virtual List<PrivilegeRegionVal> RegionVals { get; set; }
        public override string ToString()
        {
            return StartVal.GetValueOrDefault().ToString("f2") + EndVal.GetValueOrDefault().ToString("f2");
        }
    }
}
