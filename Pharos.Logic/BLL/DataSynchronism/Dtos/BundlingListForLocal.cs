using Pharos.Utility;
using System;

namespace Pharos.Logic.BLL.DataSynchronism.Dtos
{
    /// <summary>
    /// 捆绑销售清单： 用于管理本系统的商品捆绑促销清单信息（主表： Bundling）
    /// </summary>
    [Excel("捆绑商品信息")]
    public class BundlingListForLocal
    {
        
        [Excel("促销ID", 1)]

        /// <summary>
        /// 促销 ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CommodityId { get; set; }
        [Excel("商品条码", 2)]
        
        /// <summary>
        /// 商品条码
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        public string Barcode { get; set; }
        [Excel("每捆数量", 3)]

        /// <summary>
        /// 每捆数量
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public int Number { get; set; }
    }
}
