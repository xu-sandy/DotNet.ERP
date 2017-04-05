using Pharos.Utility;
using System;

namespace Pharos.Logic.BLL.DataSynchronism.Dtos
{
    /// <summary>
    /// 捆绑销售
    /// </summary>
    [Excel("捆绑信息")]

    public class BundlingForLocal
    {
        [Excel("促销ID", 1)]
        /// <summary>
        /// 促销 ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CommodityId { get; set; }
        [Excel("新捆绑条码", 2)]
        
        /// <summary>
        /// 新捆绑条码
        /// [长度：30]
        /// </summary>
        public string NewBarcode { get; set; }
        [Excel("捆绑价", 3)]

        /// <summary>
        /// 捆绑价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal BundledPrice { get; set; }
        [Excel("总捆数", 4)]
        /// <summary>
        /// 总捆数
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public int TotalBundled { get; set; }
    }
}
