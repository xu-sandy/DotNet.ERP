using System.Collections.Generic;

namespace Pharos.POS.Retailing.Models.ApiReturnResults
{
    public class DayReportResult
    {
        /// <summary>
        /// 收银员记录
        /// </summary>
        public List<SalesmanDayReportResult> SalesmanRecords { get; set; }
        /// <summary>
        /// 全员合计
        /// </summary>
        public List<DayReportDetailItem> Summary { get; set; }

    }
}
