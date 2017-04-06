using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 调价日志
    /// </summary>
    public class ChangePriceLog:BaseEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// 操作类型(1.增2.删3.改)
        /// </summary>
        public short OperType { get; set; }
        /// <summary>
        /// 业务类型:1.批发价,2-进价,3-加盟,4-售价,5-一品多价
        /// </summary>
        public short? BusinessType { get; set; }
        /// <summary>
        /// 批次
        /// </summary>
        public string BatchNo { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }
        /// <summary>
        /// 旧售价
        /// </summary>
        public decimal OldPrice { get; set; }
        /// <summary>
        /// 新售价
        /// </summary>
        public decimal NewPrice { get; set; }
        /// <summary>
        /// 门店
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDT { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string CreateUID { get; set; }
    }
}
