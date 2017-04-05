using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 批发库存消减历史表
    /// </summary>
    public class OutboundGoodsHistory
    {
        /// <summary>
        /// 记录ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public string OutboundId { get; set; }
        /// <summary>
        /// 源商品条码
        /// </summary>
        public string StorageBarcode { get; set; }
        /// <summary>
        /// 消减模式（1、批发量消减；2、组合商品消减；3、父商品拆分；4、拆分子商品入库;5、一品多码；）
        /// </summary>
        public int Mode { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Number { get; set; }
        /// <summary>
        /// 批发条码
        /// </summary>
        public string SaleBarcode { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime CreateDt { get; set; }

        public string CreateUid { get; set; }

        public string StoreId { get; set; }
    }
}
