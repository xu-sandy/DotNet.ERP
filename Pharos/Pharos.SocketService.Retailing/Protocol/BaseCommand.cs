using Pharos.SocketService.Retailing.Models;
using Pharos.SocketService.Retailing.Protocol;
using SuperSocket.SocketBase.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SocketService.Retailing.Protocol
{
    public abstract class BaseCommand : ICommand<SocketSession, SocketRequestInfo>
    {

        public BaseCommand(byte version, byte[] code)
        {
            Route = new CommandRule()
            {
                Entry = version,
                Command = code
            };
            CommandRuleProvider = new CommandRuleProvider();
        }
        public abstract void ExecuteCommand(SocketSession session, SocketRequestInfo requestInfo);

        public string Name
        {
            get { return CommandRuleProvider.GetCommandName(Route); }
        }

        public CommandRule Route { get; protected set; }

        public ICommandRuleProvider CommandRuleProvider { get; protected set; }
    }
}
