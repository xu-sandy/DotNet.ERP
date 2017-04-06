using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public class ProductSplitLog
    {
        public int Id { get; set; }
        /// <summary>
        /// 源条码
        /// </summary>
        public string SourceBarcode { get; set; }
        /// <summary>
        /// 剩余数
        /// </summary>
        public decimal ResidueNum { get; set; }
        /// <summary>
        /// 售价
        /// </summary>
        public decimal SysPrice { get; set; }
        public decimal BuyPrice { get; set; }
        public string CreateUID { get; set; }
        public DateTime CreateDT { get; set; }
    }
}
