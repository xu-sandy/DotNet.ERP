using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SocketService.Retailing.Protocol
{
    /// <summary>
    /// 命令跳转规则
    /// </summary>
    public class CommandRule
    {
        /// <summary>
        /// 命令入口
        /// </summary>
        public byte Entry { get; set; }
        /// <summary>
        /// 命令代码
        /// </summary>
        public byte[] Command { get; set; }
    }
}
