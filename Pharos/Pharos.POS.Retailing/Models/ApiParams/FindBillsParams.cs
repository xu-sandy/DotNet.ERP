using Pharos.POS.Retailing.Models.ApiReturnResults;
using System;

namespace Pharos.POS.Retailing.Models.ApiParams
{
    public class FindBillsParams : BaseApiParams
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 范围
        /// </summary>
        public Range Range { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public string PaySn { get; set; }

        public string QueryMachineSn { get; set; }
        public string Cashier { get; set; }

    }
}
