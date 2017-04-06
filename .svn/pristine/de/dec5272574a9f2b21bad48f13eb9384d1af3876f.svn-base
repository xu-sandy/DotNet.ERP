using Pharos.SocketService.Retailing.Protocol;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SocketService.Retailing
{
    public class SocketServer : AppServer<SocketSession, SocketRequestInfo>
    {
        public SocketServer()
            : base(new SocketFilterFactory())
        {
            CommandRuleProvider = new CommandRuleProvider();
            this.Setup(2020);
        }

        public ICommandRuleProvider CommandRuleProvider { get; set; }

 
        public override bool Start()
        {
            return base.Start();
        }
        public override void Stop()
        {
            base.Stop();
        }
    }
}
