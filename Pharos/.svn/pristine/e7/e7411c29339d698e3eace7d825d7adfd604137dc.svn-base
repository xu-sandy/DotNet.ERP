using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.SuperSocketClientProtocol
{
    public class DefaultRouteProvider : IRouteProvider
    {
        public DefaultRouteProvider(int routeLength)
        {
            RouteLength = routeLength;
        }
        public string GetRoute(byte[] routeCode)
        {
            return BitConverter.ToString(routeCode);
        }

        public int RouteLength { get; private set; }
    }
}
