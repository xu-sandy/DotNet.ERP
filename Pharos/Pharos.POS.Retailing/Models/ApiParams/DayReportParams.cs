﻿using Pharos.POS.Retailing.Models.ApiReturnResults;
using System;

namespace Pharos.POS.Retailing.Models.ApiParams
{
    public class DayReportParams:BaseApiParams
    {
        /// <summary>
        /// 范围（0：本机，1：门店）
        /// </summary>
        public Range Range { get; set; }
        /// <summary>
        /// 模式（0：日结，1：月结）
        /// </summary>
        public DayReportMode Mode { get; set; }
        /// <summary>
        /// 日结日期
        /// </summary>
        public DateTime Date { get; set; }
        public DateTime EndDate { get; set; }
    }
}
