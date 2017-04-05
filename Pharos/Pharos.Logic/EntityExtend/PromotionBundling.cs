using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public class PromotionBundling : BasePromotion
    {
        /// <summary>
        /// 新的条码
        /// </summary>
        public string NewBarcode { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal BundledPrice { get; set; }
        public int TotalBundled { get; set; }

        public bool GenerateCode { get; set; }
    }
}
