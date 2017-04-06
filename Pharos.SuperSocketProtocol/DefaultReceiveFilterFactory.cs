using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.SuperSocketProtocol
{
    public class DefaultReceiveFilterFactory : IReceiveFilterFactory<SockectRequestMessage>
    {
        public IReceiveFilter<SockectRequestMessage> CreateFilter(IAppServer appServer, IAppSession appSession, IPEndPoint remoteEndPoint)
        {
            var server = (SocketServer)appServer;
            IReceiveFilter<SockectRequestMessage> filter = new DefaultRouteReceiveFilter(server.RouteProvider);
            return filter;
        }
    }
}
