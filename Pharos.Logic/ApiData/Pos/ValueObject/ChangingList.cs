using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.ValueObject
{
    public class ChangingList
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal ChangeNumber { get; set; }
        /// <summary>
        /// 系统售价
        /// </summary>
        public decimal SysPrice { get; set; }
        public decimal Total { get; set; }

        public string Unit { get; set; }
        public string RecordId { get; set; }
        public int ProductType { get; set; }
    }
}
