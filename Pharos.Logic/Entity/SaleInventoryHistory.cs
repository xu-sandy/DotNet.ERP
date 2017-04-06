using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 销售库存消减历史表
    /// </summary>
    public class SaleInventoryHistory:BaseEntity
    {
        /// <summary>
        /// 记录ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 支付流水号
        /// </summary>
        public string PaySn { get; set; }
        /// <summary>
        /// 实存条码
        /// </summary>
        public string StorageBarcode { get; set; }
        /// <summary>
        /// 消减模式（1、销售量消减；2、父商品拆分；3、组合商品消减；4、整单退出库存回撤，5、拆分子商品入库,6、退货库存回撤，7、修改订单库存回撤,8、修改订单新增商品消减）
        /// </summary>
        public int Mode { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Number { get; set; }
        /// <summary>
        /// 相关库存记录
        /// </summary>
        public int InventoryId { get; set; }
        /// <summary>
        /// 销售条码
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
