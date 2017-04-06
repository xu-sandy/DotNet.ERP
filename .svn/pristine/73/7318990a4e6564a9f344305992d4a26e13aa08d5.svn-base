using Pharos.Utility;
using System;

namespace Pharos.Logic.BLL.DataSynchronism.Dtos
{

    /// <summary>
    /// 产品信息表
    /// </summary>
    [Excel("商品信息")]
    public partial class ProductInfoForLocal
    {
        [ExcelField(@"^[0-9]{1,20}$###货号为1~20位数字")]

        [Excel("货号", 1)]

        /// <summary>
        /// 货号（全局唯一）
        /// [长度：20]
        /// [不允许为空]
        /// </summary>
        public string ProductCode { get; set; }
        [LocalKey]
        [Excel("Barcode", 2)]
        [ExcelField(@"^[0-9]{5,13}$###条形码为5~13位数字")]

        /// <summary>
        /// 条形码（全局唯一）
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        public string Barcode { get; set; }
        [Excel("品名", 3)]
        [ExcelField(@"^[\s,\S]{1,50}$###品名必填且不能超过50个字符")]

        /// <summary>
        /// 品名
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        public string Title { get; set; }
        [Excel("规格", 4)]
        [ExcelField(@"^[\s,\S]{0,50}$###规格不能超过50个字符")]

        /// <summary>
        /// 规格
        /// [长度：50]
        /// </summary>
        public string Size { get; set; }
        [Excel("品牌", 5)]
        [ExcelField(@"^[0-9]{0,10}$###品牌SN为数字且大于10位")]

        /// <summary>
        /// 品牌SN
        /// [长度：10]
        /// [默认值：((-1))]
        /// </summary>
        public int? BrandSN { get; set; }

        [Excel("品类SN", 6)]
        [ExcelField(@"^[0-9]{0,10}$###品类SN（子类）为数字且大于10位")]

        /// <summary>
        /// 品类SN
        /// [长度：10]
        /// [默认值：((-1))]
        /// </summary>
        public int CategorySN { get; set; }
        [Excel("计量大单位ID", 7)]
        [ExcelField(@"^[0-9]{0,10}$###计量大单位ID为数字且大于10位")]

        /// <summary>
        /// 计量大单位ID（来自数据字典表）
        /// [长度：10]
        /// [默认值：((-1))]
        /// </summary>
        public int BigUnitId { get; set; }
        [Excel("计量小单位ID", 8)]
        [ExcelField(@"^[0-9]{0,10}$###计量小单位ID为数字且大于10位")]

        /// <summary>
        /// 计量小单位ID（来自数据字典表） 
        /// [长度：10]
        /// [默认值：((-1))]
        /// </summary>
        public int SubUnitId { get; set; }
        [Excel("进价", 9)]

        [ExcelField(@"^[0-9]*[.]{0,1}[0-9]*$###进价格式错误")]

        /// <summary>
        /// 进价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal BuyPrice { get; set; }
        [Excel("系统售价", 10)]

        /// <summary>
        /// 系统售价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [ExcelField(@"^[0-9]*[.]{0,1}[0-9]*$###系统售价格式错误")]
        public decimal SysPrice { get; set; }
        [Excel("产品性质", 11)]
        [ExcelField(@"^[0,1]$###产品性质值范围（0:单品、1:组合）")]

        /// <summary>
        /// 产品性质（0:单品、1:组合、2：拆分） 
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
        [Excel("产品状态", 13)]
        [ExcelField(@"^[0,1]$###产品状态值范围（0:已下架、1:已上架）")]

        /// <summary>
        /// 产品状态（0:已下架、1:已上架）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public short State { get; set; }
        [Excel("前台优惠", 14)]
        [ExcelField(@"^[0,1]{0,1}$###前台优惠值范围（1:允许调价、0:不允许调价)")]

        /// <summary>
        /// 前台优惠（1:允许调价、0:不允许调价)
        /// </summary>
        public short? Favorable { get; set; }
        [Excel("计价方式", 15)]
        [ExcelField(@"^[1,2]{0,1}$###计价方式值范围（1:计件、2:称重）")]

        /// <summary>
        /// 计价方式（1:计件、2:称重）
        /// </summary>
        public short ValuationType { get; set; }
        [Excel("退货标志", 16)]
        [ExcelField(@"^[0,1]{0,1}$###退货标志范围（1:允许、0:不允许）")]

        /// <summary>
        /// 退货标志（1:允许、0:不允许）
        /// </summary>
        public short? IsReturnSale { get; set; }
        /// <summary>
        /// 一品多码
        /// </summary>
        public string Barcodes { get; set; }
        /// <summary>
        /// 多条码串
        /// </summary>
        public string BarcodeMult { get; set; }
        /// <summary>
        /// 库存量
        /// </summary>

        public decimal StockNumber { get; set; }
    }
}
