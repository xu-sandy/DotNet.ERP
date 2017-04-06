using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharos.Api.Retailing.Models.Mobile
{
    /// <summary>
    /// 销售统计
    /// </summary>
    public class SaleReportRequest:BaseApiParams
    {
        /// <summary>
        /// 销售日期
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 销售结束日期(月结)
        /// </summary>
        public string Date2 { get; set; }
        /// <summary>
        /// 统计类型（1-日结2-月结）
        /// </summary>
        public short Type { get; set; }
    }
    /// <summary>
    /// 手机帐单汇总
    /// </summary>
    public class SaleSummaryRequest : BaseParams
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 1-指定月份，2-指定日期，3-指定时间
        /// </summary>
        public string Type { get; set; }
    }
}