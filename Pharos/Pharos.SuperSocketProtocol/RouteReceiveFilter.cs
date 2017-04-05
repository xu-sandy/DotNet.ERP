using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.SuperSocketProtocol
{
    /// <summary>
    /// 头部长度8位，前四位为请求命令，后四位为消息体长度
    /// </summary>
    /// <typeparam name="TRequestInfo"></typeparam>
    public abstract class RouteReceiveFilter<TRequestInfo> : FixedHeaderReceiveFilter<TRequestInfo> where TRequestInfo : IRequestInfo
    {
        public RouteReceiveFilter(IRouteProvider routeProvider)
            : base(routeProvider.RouteLength + 4)
        {
            RouteProvider = routeProvider;
        }
        public IRouteProvider RouteProvider { get; set; }

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            return BitConverter.ToInt32(header, offset + RouteProvider.RouteLength);
        }

        protected override TRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            return LoadRequestInfo(header.Array.CloneRange(header.Offset, 4), bodyBuffer.CloneRange(offset, length));
        }

        public abstract TRequestInfo LoadRequestInfo(byte[] routeCode, byte[] bodyBuffer);

    }
}
