using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public class OrderDistributionGift
    {
        public int Id { get; set; }
        public int OrderDistributionId { get; set; }
        public string Barcode { get; set; }
        /// <summary>
        /// 订货数量
        /// </summary>
        public decimal IndentNum { get; set; }
        /// <summary>
        /// 实收数量
        /// </summary>
        public decimal? ReceivedNum { get; set; }
    }
}
