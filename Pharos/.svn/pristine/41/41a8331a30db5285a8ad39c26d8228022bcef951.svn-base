using Pharos.Logic.OMS;
using Pharos.Logic.OMS.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QCT.Api.Pay.Utils
{
    /// <summary>
    /// 支付交易基础Builder
    /// </summary>
    public class BasePayBuilder
    {
        /// <summary>
        /// 初始化构造函数
        /// </summary>
        public BasePayBuilder()
        {
            PaySvc = new PayService();
            LogEngine = new LogEngine();
        }
        /// <summary>
        /// 支付交易Service
        /// </summary>
        public PayService PaySvc { get; set; }
        /// <summary>
        /// 日志记录引擎
        /// </summary>
        public LogEngine LogEngine { get; set; }
    }
}