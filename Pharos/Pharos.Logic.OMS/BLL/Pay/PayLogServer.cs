using QCT.Pay.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.BLL.Pay
{
    /// <summary>
    /// 支付日志Server
    /// </summary>
    public class PayLogServer
    {
        /// <summary>
        /// 记录测试日志
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteInfoAsTestLog(string msg)
        {
            if (PayConfig.IsSaveTestLog)
            {
                WriteInfo(msg);
            }
        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteInfo(string msg)
        {
            var log = new LogEngine();
            log.WriteInfo(msg, LogModule.支付交易);
        }
        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteError(string msg, Exception ex)
        {
            var log = new LogEngine();
            log.WriteError(msg, ex, LogModule.支付交易);
        }
    }
}
