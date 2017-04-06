using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharos.Api.Retailing.Models.Mobile
{
    public class SaleBillRequest : BaseListParams
    {
        /// <summary>
        /// 商户号
        /// </summary>
        public int CID { get; set; }
        /// <summary>
        /// 门店ID
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 类型(0-正常，1-换货，2-退货,3-退单)
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 时间(0-今日，1-昨天，2-本周)
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 收银员
        /// </summary>
        public string Cashier { get; set; }
        /// <summary>
        /// 导购员
        /// </summary>
        public string Saler { get; set; }
    }
}