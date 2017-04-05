using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SocketService.Retailing.Protocol
{
    /// <summary>
    /// 命令路由解析器
    /// </summary>
    public class CommandRuleProvider : ICommandRuleProvider
    {
        private static string RouteFormatString = string.Empty;
        /// <summary>
        /// 初始化器
        /// </summary>
        /// <param name="routeFormatString">格式化命令为字符串</param>
        public CommandRuleProvider(string routeFormatString = "{0}|{1}{2}")
        {
            RouteFormatString = routeFormatString;
        }
        /// <summary>
        /// 获取命令格式化名称
        /// </summary>
        /// <param name="routeRule"></param>
        /// <returns></returns>
        public string GetCommandName(CommandRule routeRule)
        {
            Verfy(routeRule, true);
            return string.Format(RouteFormatString, routeRule.Entry, routeRule.Command[0], routeRule.Command[1]);
        }

        /// <summary>
        /// 命令流长度
        /// </summary>
        public int BytesLength
        {
            get
            {
                return 3;
            }
        }

        /// <summary>
        /// 验证路由是否符合规则
        /// </summary>
        /// <param name="routeRule">路由</param>
        /// <param name="enableThrowException">验证失败是否抛出异常，默认为false</param>
        /// <returns>验证结果</returns>
        public bool Verfy(CommandRule routeRule, bool enableThrowException = false)
        {
            if (routeRule.Entry == 0)
            {
                if (enableThrowException)
                {
                    throw new StackOverflowException("命令路由起点不能为0x00！");
                }
                else
                {
                    return false;
                }
            }
            if (routeRule.Command.Length != 2)
            {
                if (enableThrowException)
                {
                    throw new StackOverflowException("命令路由长度无效！");
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public byte[] DataSyncPackageCode
        {
            get { return new byte[] { 0x00, 0x00, 0x01 }; }
        }

        public byte[] MessageCode
        {
            get { return new byte[] { 0x00, 0x00, 0x00 }; }
        }
    }
}
