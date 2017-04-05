using Pharos.SocketService.Retailing.Protocol;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SocketService.Retailing
{
    public class SocketRequestInfo : RequestInfo<byte[]>
    {
        public CommandRule Route { get; private set; }

        public ICommandRuleProvider CommandRuleProvider { get; private set; }
        public SocketRequestInfo(CommandRule routeRule, ICommandRuleProvider commandRuleProvider, byte[] requestBody)
        {
            Route = routeRule;
            CommandRuleProvider = commandRuleProvider;
            var key = CommandRuleProvider.GetCommandName(routeRule);
            Initialize(key, requestBody);
        }
    }
}
