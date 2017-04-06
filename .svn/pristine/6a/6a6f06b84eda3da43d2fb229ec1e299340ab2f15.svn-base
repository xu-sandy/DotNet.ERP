using Newtonsoft.Json;
using Pharos.Logic.BLL.DataSynchronism;
using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.LocalEntity
{
    [Excel("换货明细")]

    public class SalesReturnsDetailed : BaseEntity, ICanUploadEntity
    {
        public Int64 Id { get; set; }
        [Excel("退换货ID", 1)]
        [ExcelField(@"^[0-9,a-z,A-Z]{1,40}$###退换货ID应为Guid")]

        [LocalKey]

        /// <summary>
        /// 退换货ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string ReturnId { get; set; }
        [Excel("商品条码", 2)]
        [ExcelField(@"^[0-9]{5,13}$###商品条码5~13位数字")]

        /// <summary>
        /// 商品条码
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        [LocalKey]

        public string Barcode { get; set; }
        [Excel("票据单号", 3)]
        [ExcelField(@"^[0-9]{1,30}$###票据单号最多30位数字")]

        /// <summary>
        /// 票据单号 
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        public string ReceiptsNumber { get; set; }
        [Excel("数量", 4)]
        [ExcelField(@"^[0-9]*[.]{0,1}[0-9]*$###数量格式错误")]

        /// <summary>
        /// 数量
        /// [长度：10]
        /// 默认值：1
        /// [不允许为空]
        /// </summary>
        public decimal Number { get; set; }
        [Excel("单价", 5)]
        [ExcelField(@"^[0-9]*[.]{0,1}[0-9]*$###单价格式错误")]

        /// <summary>
        /// 单价
        /// [长度：10]
        /// 默认值：0
        /// [不允许为空]
        /// </summary>
        public decimal Price { get; set; }
        [Excel("实销价", 6)]
        [ExcelField(@"^[0-9]*[.]{0,1}[0-9]*$###实销价格式错误")]

        /// <summary>
        /// 实销价 
        /// 默认值：0
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        public decimal TradingPrice { get; set; }
        [JsonIgnore]

        public bool IsUpload { get; set; }
        [JsonIgnore]


        public DateTime CreateDT { get; set; }
    }
}
