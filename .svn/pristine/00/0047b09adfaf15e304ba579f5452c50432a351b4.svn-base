using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.SuperSocketProtocol
{
    public class SocketServer : AppServer<SocketSession, SockectRequestMessage>
    {
        public SocketServer(IRouteProvider routeProvider)
            : base(new DefaultReceiveFilterFactory())
        {
            RouteProvider = routeProvider;
        }
        public IRouteProvider RouteProvider { get; private set; }

        public static SocketServer InitServer(IRouteProvider routeProvider, int port)
        {
            SocketServer appServer = new SocketServer(routeProvider);
            if (!appServer.Setup(port)) //Setup with listening port
            {
                throw new Exception("Failed to setup!");
            }
            if (!appServer.Start())
            {
                throw new Exception("Failed to start!");
            }
            return appServer;
        }
    }
}
