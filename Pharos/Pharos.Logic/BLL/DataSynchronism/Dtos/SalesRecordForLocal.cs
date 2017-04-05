using Pharos.Utility;

namespace Pharos.Logic.Entity.LocalEntity
{
    /// <summary>
    /// 促销活动量记录
    /// </summary>
    [Excel("促销活动量记录", 1)]

    public class SalesRecordForLocal
    {
        /// <summary>
        /// 促销Id
        /// </summary>
        [Excel("促销Id", 1)]
        public string CommodityId { get; set; }
        /// <summary>
        /// 商店Id
        /// </summary>
        [Excel("商店Id", 1)]
        public string StoreId { get; set; }
        /// <summary>
        /// 销售数
        /// </summary>
        [Excel("销售数", 1)]
        public decimal Number { get; set; }
    }
}
