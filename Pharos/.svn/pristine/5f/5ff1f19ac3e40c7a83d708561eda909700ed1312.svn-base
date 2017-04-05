using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using Pharos.SocketClient.Retailing.Extensions;
using System.Text;

namespace Pharos.SocketClient.Retailing.Protocol.ReceiveFilters
{
    public abstract class CustomFixedHeaderReceiveFilter<TPackageInfo> : FixedHeaderReceiveFilter<TPackageInfo> where TPackageInfo : IPackageInfo
    {
        public CustomFixedHeaderReceiveFilter()
            : base(8)
        {
        }

        protected override int GetBodyLengthFromHeader(IBufferStream bufferStream, int length)
        {
            var buffer = bufferStream.Read();
            return BitConverter.ToInt32(buffer, 4);
        }

        public override TPackageInfo ResolvePackage(IBufferStream bufferStream)
        {
            var buffer = bufferStream.Read();

            var cmdCode = buffer.CloneRange(0, 4);
            var length = BitConverter.ToInt32(buffer.CloneRange(4, 4), 0);
            var body = buffer.CloneRange(8, length);

            return LoadRequestInfo(cmdCode, body);
        }
        public abstract TPackageInfo LoadRequestInfo(byte[] cmdCode, byte[] bodyBuffer);

    }
}
