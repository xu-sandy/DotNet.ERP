﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 库存结余记录日志表
    /// </summary>
    public class InventoryRecord:BaseEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        ///  门店Id
        /// </summary>

        public string StoreId { get; set; }
        /// <summary>
        ///  条码
        /// </summary>

        public string Barcode { get; set; }
        /// <summary>
        /// 数量
        /// </summary>

        public decimal Number { get; set; }

        /// <summary>
        /// 销售均价
        /// </summary>
        public decimal AveragePrice { get; set; }

        /// <summary>
        /// 时间
        /// </summary>

        public DateTime CreateDT { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>

        public string CreateUID { get; set; }
        /// <summary>
        ///  来源标识（1、总部入库；2、总部出库；3、调拨入库；4、调拨出库；5、门店售后退换货入库；6、门店售后换货出库；7、拆分子商品入库；8、拆分父商品消减；9、销售商品消减；
        ///  10、销售组合商品消减;11、门店收货；12、门店预约退换；13、总部批发出库;14、总部退货;15、库存纠正(正、负)；16、总部报损）
        /// </summary>

        public int Source { get; set; }
        /// <summary>
        /// 入库或配送ID
        /// </summary>
        public string OperatId { get; set; }
        /// <summary>
        /// 操作类型(1-添加2-相减)
        /// </summary>
        [NotMapped]
        public short OperatType { get; set; }
    }
}
