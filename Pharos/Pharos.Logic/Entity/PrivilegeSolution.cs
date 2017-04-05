using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public partial class PrivilegeSolution:BaseEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// 方案名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 返利模式
        /// </summary>
        public int ModeSN { get; set; }
        /// <summary>
        /// 返利供应商
        /// </summary>
        public string SupplierIds { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Memo { get; set; }
        [Pharos.Utility.Exclude]
        public string OperatorUID { get; set; }
        [Pharos.Utility.Exclude]
        public DateTime CreateDT { get; set; }

        
    }
}
