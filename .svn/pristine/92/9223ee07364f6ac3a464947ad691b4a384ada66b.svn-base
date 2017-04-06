using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharos.Api.Retailing.Models.Mobile
{
    public class ProductRequest
    {
        /// <summary>
        /// 公司授权码
        /// </summary>
        public int CID { get; set; }
        /// <summary>
        /// 第一级分类
        /// </summary>
        public int FirstSN { get; set; }
        /// <summary>
        /// 查询条码
        /// </summary>
        public string Barcode { get; set; }
    }
    public class ProductListRequest : ProductRequest
    {
        public BaseListParams BaseList { get; set; }
        /// <summary>
        /// 第二级分类
        /// </summary>
        public int? SecondSN { get; set; }
        /// <summary>
        /// 第三级分类
        /// </summary>
        public int? ThreeSN { get; set; }
    }
}