using Pharos.Logic.ApiData.Pos.Sale.Barcodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Sale.Marketings
{
    public class MarketingContext
    {
        /// <summary>
        /// 匹配范围
        /// </summary>
        public IEnumerable<IBarcode> MatchRanges { get; set; }

        /// <summary>
        /// 匹配规则
        /// </summary>
        public ActiveMarketingRule MarketingRule { get; set; }

        /// <summary>
        /// 匹配商品数量
        /// </summary>
        public decimal ProductCount { get; set; }
        /// <summary>
        /// 匹配商品总额
        /// </summary>
        public decimal ProductTotal { get; set; }

        static MarketingContext _Empty = default(MarketingContext);
        public static MarketingContext Empty
        {
            get
            {
                return _Empty;
            }
        }

    }
}
