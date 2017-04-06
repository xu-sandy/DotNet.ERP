using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Barcode.Retailing.Dtos
{
    public class ProductBrandDto
    {
        /// <summary>
        /// 品牌编号（全局唯一)
        /// </summary>
        public int BrandSN { get; set; }

        /// <summary>
        /// 品牌名称
        /// </summary>
        public string Title { get; set; }

    }
}
