using Pharos.Logic.ApiData.Pos.Sale;
using System;

namespace Pharos.Api.Retailing.Models.Pos
{
    public class DayReportRequest : BaseApiParams
    {
        /// <summary>
        /// 范围（0：本机，1：门店）
        /// </summary>
        public Range Range { get; set; }
        /// <summary>
        /// 模式（0：日结，1：月结）
        /// </summary>
        public Mode Mode { get; set; }
        /// <summary>
        /// 日结日期
        /// </summary>
        public DateTime Date { get; set; }
        public DateTime EndDate { get; set; }
    }
}