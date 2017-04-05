using Pharos.SuperSocketClientProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Pharos.MessageTransferAgenClient.DomainEvent;

namespace Pharos.MessageTransferAgenClient
{
    public class MessageClient : SocketClient
    {
        public MessageClient()
            : base(new DefaultRouteProvider(4))
        {
        }
        static MessageClient current;
        public static MessageClient Current
        {
            get
            {
                if (current == null)
                {
                    current = new MessageClient();
                    current.Initialize();
                    Task.Factory.StartNew(() =>
                    {
                        EventAggregator.RefreshSubscribe(new TimeSpan(1, 0, 0));
                    });

                }
                if (!current.IsConnected)
                {
                    var ipaddress = Dns.GetHostAddresses(MessageSettings.Current.Host);

                    if (ipaddress.Length > 0)
                    {
                        var connected = current.ConnectAsync(new IPEndPoint(ipaddress.FirstOrDefault(), MessageSettings.Current.Port));
                        connected.Wait();
                    }
                    else
                    {
                        throw new ArgumentException("代理消息服务，主机名称或者IP无效！");
                    }
                }
                return current;
            }
        }
    }
}
