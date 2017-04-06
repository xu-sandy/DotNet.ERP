using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase.Protocol;
using System;
using Pharos.SocketService.Retailing.Extensions;

namespace Pharos.SocketService.Retailing.Protocol.ReceiveFilters
{
    /// <summary>
    /// 头部长度8位，前四位为请求命令，后四位为消息体长度
    /// </summary>
    /// <typeparam name="TRequestInfo"></typeparam>
    public abstract class CustomFixedHeaderReceiveFilter<TRequestInfo> : FixedHeaderReceiveFilter<TRequestInfo> where TRequestInfo : IRequestInfo
    {
        public CustomFixedHeaderReceiveFilter()
            : base(8)
        {
        }
        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            return BitConverter.ToInt32(header, offset + 4);
        }

        protected override TRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            return LoadRequestInfo(header.Array.CloneRange(header.Offset, 4), bodyBuffer.CloneRange(offset, length));
        }

        public abstract TRequestInfo LoadRequestInfo(byte[] cmdCode, byte[] bodyBuffer);
    }
}
