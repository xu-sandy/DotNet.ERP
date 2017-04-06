using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SocketService.Retailing.Protocol
{
    public class SocketFilterFactory : IReceiveFilterFactory<SocketRequestInfo>
    {

        IReceiveFilter<SocketRequestInfo> IReceiveFilterFactory<SocketRequestInfo>.CreateFilter(SuperSocket.SocketBase.IAppServer appServer, SuperSocket.SocketBase.IAppSession appSession, System.Net.IPEndPoint remoteEndPoint)
        {
            var castedServer = (SocketServer)appServer;
            var result = new SocketReceiveFilter(castedServer.CommandRuleProvider);
            return result;
        }
    }
}
