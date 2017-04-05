using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 返利计算明细
    /// </summary>
    public class PrivilegeCalcDetail
    {
        public int Id { get; set; }
        public string IndentOrderId { get; set; }
        public string Barcode { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string PartName { get; set; }
        /// <summary>
        /// 品类
        /// </summary>
        public int CategorySN { get; set; }
        /// <summary>
        /// 品类
        /// </summary>
        public string CategoryTitle { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime OrderDate { get; set; }
        /// <summary>
        /// 收货时间
        /// </summary>
        public DateTime? TakeDate { get; set; }
        /// <summary>
        /// 收货量
        /// </summary>
        public decimal TakeNum { get; set; }
        /// <summary>
        /// 进价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 小计
        /// </summary>
        public decimal SubTotal { get; set; }
        public int PrivilegeCalcId { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual PrivilegeCalc Calc { get; set; }
    }
}
