using Pharos.SocketService.Retailing.Protocol;
using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SocketService.Retailing
{
    public class SocketSession : AppSession<SocketSession, SocketRequestInfo>
    {
        internal protected static readonly Encoding defaultEncoding = Encoding.UTF8;
        public SocketServer IhAppServer { get { return AppServer as SocketServer; } }



        public byte[] Format(byte[] body)
        {
            var begin = SocketReceiveFilter.BeginMark;
            var end = SocketReceiveFilter.EndMark;
            var rawMsg = new byte[begin.Length + body.Length + end.Length];
            Array.Copy(begin, 0, rawMsg, 0, begin.Length);
            Array.Copy(body, 0, rawMsg, begin.Length, body.Length);
            Array.Copy(end, 0, rawMsg, begin.Length + body.Length, end.Length);
            return rawMsg;
        }
    }
}
