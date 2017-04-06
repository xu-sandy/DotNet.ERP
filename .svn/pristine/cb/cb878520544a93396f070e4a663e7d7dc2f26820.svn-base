using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public class PrivilegeCalcResult
    {
        public int Id { get; set; }
        public decimal StartVal { get; set; }
        public decimal EndVal { get; set; }
        /// <summary>
        /// 条码或类别
        /// </summary>
        public string BarcodeOrCategorySN { get; set; }
        /// <summary>
        /// 类型(1-商品条码2-商品类型)
        /// </summary>
        public short Type { get; set; }
        /// <summary>
        /// 设定值
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// 合计数
        /// </summary>
        public decimal TotalNum { get; set; }
        public int PrivilegeCalcId { get; set; }
        public virtual PrivilegeCalc Calc { get; set; }
    }
}
