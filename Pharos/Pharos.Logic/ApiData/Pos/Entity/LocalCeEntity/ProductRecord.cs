﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity
{
    public class ProductRecord : BaseEntity
    {
        /// <summary>
        /// 货号（全局唯一）
        /// [长度：20]
        /// [不允许为空]
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 条形码（全局唯一）
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 品名
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 规格
        /// [长度：50]
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// 品牌SN
        /// [长度：10]
        /// [默认值：((-1))]
        /// </summary>
        public int? BrandSN { get; set; }

        /// <summary>
        /// 主供应商ID
        /// </summary>
        public string SupplierId { get; set; }

        /// <summary>
        /// 产地ID（来自城市ID） 
        /// [长度：10]
        /// [默认值：((-1))]
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// 品类SN（大类）
        /// [长度：10]
        /// [默认值：((-1))]
        /// </summary>
        public int CategorySN { get; set; }
        /// <summary>
        /// 计量大单位ID（来自数据字典表）
        /// [长度：10]
        /// [默认值：((-1))]
        /// </summary>
        public int BigUnitId { get; set; }

        /// <summary>
        /// 计量小单位ID（来自数据字典表） 
        /// [长度：10]
        /// [默认值：((-1))]
        /// </summary>
        public int SubUnitId { get; set; }

        /// <summary>
        /// 进价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal BuyPrice { get; set; }

        /// <summary>
        /// 系统售价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        //[Excel("系统售价", 12)]
        public decimal SysPrice { get; set; }

        /// <summary>
        /// 产品性质（0:单品、1:组合、2:拆分） 
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public short Nature { get; set; }

        /// <summary>
        /// 对应条码
        /// [长度：30]
        /// </summary>
        public string OldBarcode { get; set; }
        /// <summary>
        /// 可售数量
        /// </summary>
        public decimal? SaleNum { get; set; }

        /// <summary>
        /// 物价员（UID）
        /// [长度：40]
        /// [默认值：((-1))]
        /// </summary>
        public string RaterUID { get; set; }

        /// <summary>
        /// 产品状态（0:已下架、1:已上架）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public short State { get; set; }
        /// <summary>
        /// 箱规
        /// </summary>
        public string BoxBoard { get; set; }
        /// <summary>
        /// 前台优惠（1:允许调价、0:不允许调价)
        /// </summary>
        public short? Favorable { get; set; }
        /// <summary>
        /// 保质期（0:不限）
        /// </summary>
        public short Expiry { get; set; }
        /// <summary>
        /// 保质期单位（1:天、2:月、3:年）
        /// </summary>
        public short? ExpiryUnit { get; set; }
        /// <summary>
        /// 生产厂家
        /// </summary>
        public string Factory { get; set; }
        /// <summary>
        /// 计价方式（1:计件、2:称重）
        /// </summary>
        public short ValuationType { get; set; }
        /// <summary>
        /// 退货标志（1:允许、0:不允许）
        /// </summary>
        //[Excel("退货标志", 18)]
        public short? IsReturnSale { get; set; }
        /// <summary>
        /// 订货标志（1:允许、0:不允许）
        /// </summary>
        public short? IsAcceptOrder { get; set; }
        /// <summary>
        /// 库存预警（数量）
        /// </summary>
        public short? InventoryWarning { get; set; }
        /// <summary>
        /// 保质期预警（天）
        /// </summary>
        public short? ValidityWarning { get; set; }



        /// <summary>
        /// 批发价
        /// </summary>
        public decimal TradePrice { get; set; }
        /// <summary>
        /// 加盟价
        /// </summary>
        public decimal JoinPrice { get; set; }
        /// <summary>
        /// 一品多码
        /// </summary>
        public string Barcodes { get; set; }
        /// <summary>
        /// 多条码串
        /// </summary>
        public string BarcodeMult { get; set; }
        /// <summary>
        /// 进项税率
        /// </summary>
        public decimal? StockRate { get; set; }
        /// <summary>
        /// 销售税率
        /// </summary>
        public decimal? SaleRate { get; set; }

        public DateTime CreateDT { get; set; }
        public string BrandName { get; set; }
        public string CategoryPath { get; set; }
        public string CategoryPathName { get; set; }
        public string CategoryName { get; set; }
        public string Unit { get; set; }
        public decimal Inventory { get; set; }
    }
}