using SuperSocket.SocketBase.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SuperSocketProtocol
{
    public abstract class CommandBase : ICommand<SocketSession, SockectRequestMessage>
    {
        public CommandBase(params byte[] routeCode)
            : this(new DefaultRouteProvider(4), routeCode)
        {
        }
        public CommandBase(IRouteProvider routeProvider, params byte[] routeCode)
        {
            if (routeCode == null || routeProvider.RouteLength != routeCode.Length)
            {
                throw new ArgumentException("路由与路由规则不匹配！");
            }
            RouteProvider = routeProvider;
            RouteCode = routeCode;
        }
        public void ExecuteCommand(SocketSession session, SockectRequestMessage requestInfo)
        {
            Execute(session.SocketServer, session, requestInfo);
        }
        public abstract void Execute(SocketServer server, SocketSession session, SockectRequestMessage requestInfo);
        public string Name
        {
            get { return RouteProvider.GetRoute(RouteCode); }
        }

        public IRouteProvider RouteProvider { get; private set; }

        public byte[] RouteCode { get; private set; }
    }
}
