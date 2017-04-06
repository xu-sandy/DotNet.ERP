using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SocketClient.Retailing.Extensions
{
    public static class IBufferStreamExtensions
    {
        public static byte[] Read(this IBufferStream bufferStream)
        {
            var buffer = new byte[bufferStream.Length];

            bufferStream.Read(buffer, 0, (int)bufferStream.Length);
            return buffer;
        }
    }
}
