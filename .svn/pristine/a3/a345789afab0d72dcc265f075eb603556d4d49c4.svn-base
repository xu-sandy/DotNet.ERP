using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SyncService.SyncEntities
{
    [Serializable]
    public class Bundling : SyncDataObject
    {
        /// <summary>
        /// 促销 ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CommodityId { get; set; }

        public int CompanyId { get; set; }

        /// <summary>
        /// 新捆绑条码
        /// [长度：30]
        /// </summary>
        public string NewBarcode { get; set; }
        /// <summary>
        /// 新品名
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 捆绑价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal BundledPrice { get; set; }


        /// <summary>
        /// 总捆数
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public int TotalBundled { get; set; }
        //[Pharos.Utility.Exclude]
        //public virtual CommodityPromotion Promotion { get; set; }
        /// <summary>
        /// 售价小计
        /// </summary>
        public decimal SysPrices { get; set; }
        /// <summary>
        /// 进价合计
        /// </summary>
        public decimal BuyPrices { get; set; }
    }
}
