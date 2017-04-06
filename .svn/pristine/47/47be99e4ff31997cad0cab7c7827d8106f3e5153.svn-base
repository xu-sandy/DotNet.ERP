using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 盘点日志
    /// </summary>
    public class StockTakingLog:BaseEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// 盘点批次
        /// </summary>
        public string CheckBatch { get; set; }
        public string Barcode { get; set; }
        /// <summary>
        /// 盘点数量
        /// </summary>
        public decimal Number { get; set; }
        /// <summary>
        /// 状态 1-复盘
        /// </summary>
        public short State { get; set; }

        public decimal? SysPrice { get; set; }
        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime CreateDT { get; set; }
        /// <summary>
        /// 盘点日期
        /// </summary>
        public DateTime? ActualDate { get; set; }
        /// <summary>
        /// 盘点员
        /// </summary>
        public string CheckUID { get; set; }
        /// <summary>
        /// 录入员
        /// </summary>
        public string CreateUID { get; set; }
        /// <summary>
        /// 来源（1-电脑，2-手机，3-导入）
        /// </summary>
        public short Source { get; set; }
    }
}
