using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharos.Api.Retailing.Models.Mobile
{
    public class HistoryOrderRequest : BaseParams
    {
        /// <summary>
        /// 门店
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public int Status { get; set; }
    }
}