using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 返利计算
    /// </summary>
    public class PrivilegeCalc:BaseEntity
    {
        public int Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public string SupplierId { get; set; }
        public string SupplierTitle { get; set; }
        public int PrivilegeSolutionId { get; set; }
        /// <summary>
        /// 方案名称
        /// </summary>
        public string PrivilegeSolutionTitle { get; set; }
        /// <summary>
        /// 返利模式
        /// </summary>
        public int PrivilegeModeSN { get; set; }
        public string PrivilegeModeSNTitle { get; set; }
        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal TotalMoney { get; set; }
        /// <summary>
        /// 返利(元/数量)
        /// </summary>
        public decimal PrivilegeNum { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public short State { get; set; }
        public string OperatorUID { get; set; }
        public DateTime CreateDT { get; set; }

        public virtual List<PrivilegeCalcDetail> Details { get; set; }
        public virtual List<PrivilegeCalcResult> Results { get; set; }
    }
}
