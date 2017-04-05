using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.SuperSocketProtocol
{
    public class DefaultRouteReceiveFilter : RouteReceiveFilter<SockectRequestMessage>
    {
        public DefaultRouteReceiveFilter(IRouteProvider routeProvider)
            : base(routeProvider)
        {

        }
        public override SockectRequestMessage LoadRequestInfo(byte[] routeCode, byte[] bodyBuffer)
        {
            var route = RouteProvider.GetRoute(routeCode);
            SockectRequestMessage msg = new SockectRequestMessage(route, bodyBuffer);
            return msg;
        }
    }
}
