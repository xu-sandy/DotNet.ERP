using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SuperSocketClientProtocol
{
    public abstract class RouteReceiveFilter<TPackageInfo> : FixedHeaderReceiveFilter<TPackageInfo> where TPackageInfo : SuperSocket.ProtoBase.IPackageInfo
    {
        public RouteReceiveFilter(IRouteProvider routeProvider)
            : base(routeProvider.RouteLength + 4)
        {
            RouteProvider = routeProvider;
        }
        protected override int GetBodyLengthFromHeader(IBufferStream bufferStream, int length)
        {
            var buffer = bufferStream.Read();
            return BitConverter.ToInt32(buffer, RouteProvider.RouteLength);
        }

        public override TPackageInfo ResolvePackage(IBufferStream bufferStream)
        {
            var buffer = bufferStream.Read();

            var cmdCode = buffer.CloneRange(0, RouteProvider.RouteLength);
            var length = BitConverter.ToInt32(buffer.CloneRange(RouteProvider.RouteLength, 4), 0);
            var body = buffer.CloneRange(RouteProvider.RouteLength + 4, length);

            return LoadPackageInfo(cmdCode, body);
        }


        public IRouteProvider RouteProvider { get; set; }

        public abstract TPackageInfo LoadPackageInfo(byte[] routeCode, byte[] bodyBuffer);

    }
}
