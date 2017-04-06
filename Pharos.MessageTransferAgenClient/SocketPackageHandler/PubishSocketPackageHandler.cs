using Pharos.MessageAgent.Data;
using Pharos.MessageTransferAgenClient.DomainEvent;
using Pharos.SuperSocketClientProtocol;
using Pharos.SuperSocketClientProtocol.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Pharos.MessageTransferAgenClient.SocketPackageHandler
{
    public class PubishSocketPackageHandler : BaseSocketPackageHandler
    {
        public PubishSocketPackageHandler()
            : base(0x01, 0x00, 0x00, 0x01)
        { }

        public override void Handler(SocketClient client, SockectPackageMessage package)
        {
            PubishInformaction info;
            if (TryReadPubishInformaction(package, out info))
            {
                new EventAggregator().RemotePublish(info);
            }
        }
        private bool TryReadPubishInformaction(SockectPackageMessage package, out PubishInformaction info)
        {
            info = null;
            if (package.Body.Length < 5) return false;

            var len = BitConverter.ToInt32(package.Body, 0);
            string topic;
            if (package.TryReadFromText(package.Body, 4, len, out topic))
            {
                string content;
                if (package.TryReadFromText(package.Body, 4 + len, package.Body.Length - 4 - len, out content))
                {
                    info = new PubishInformaction()
                    {
                        Topic = topic,
                        Content = content
                    };
                    return true;
                }
            }
            return false;
        }
    }
}
