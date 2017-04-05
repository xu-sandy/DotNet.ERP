using Pharos.SuperSocketClientProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.MessageTransferAgenClient.SocketPackageHandler
{
    public abstract class BaseSocketPackageHandler : ISocketPackageHandler
    {
        public BaseSocketPackageHandler(params byte[] routeCode)
        {
            RouteProvider = new DefaultRouteProvider(4);
            Route = RouteProvider.GetRoute(routeCode);
        }
        public virtual void Handler(SocketClient client, SockectPackageMessage package)
        {
        }

        public string Route { get; set; }

        public IRouteProvider RouteProvider { get; set; }
    }
}
