using System.Collections.Generic;

namespace Pharos.POS.Retailing.Models.ApiReturnResults
{
    public class DayReportDetailItem
    {
        public string Project { get; set; }

        public int Number { get; set; }

        public decimal Amount { get; set; }

        public List<PayWayItem> PayWay { get; set; }
    }
}
