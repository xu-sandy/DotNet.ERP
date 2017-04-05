using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.SuperSocketClientProtocol
{
    public class DefaultRouteReceiveFilter : RouteReceiveFilter<SockectPackageMessage>
    {
        public DefaultRouteReceiveFilter(IRouteProvider routeProvider)
            : base(routeProvider)
        {

        }

        public override SockectPackageMessage LoadPackageInfo(byte[] routeCode, byte[] bodyBuffer)
        {
            SockectPackageMessage msg = new SockectPackageMessage(RouteProvider.GetRoute(routeCode), bodyBuffer);
            return msg;
        }
    }
}
