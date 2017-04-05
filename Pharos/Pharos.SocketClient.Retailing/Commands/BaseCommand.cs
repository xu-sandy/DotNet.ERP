using Pharos.SocketClient.Retailing.CommandProviders;
using Pharos.SocketClient.Retailing.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SocketClient.Retailing.Commands
{
    public abstract class BaseCommand : ICommand
    {
        ICommandNameProvider _commandNameProvider;

        public BaseCommand(byte[] cmdCode, ICommandNameProvider commandNameProvider) 
        {
            this._commandNameProvider = commandNameProvider;
            CmdCode = cmdCode;
            Key = _commandNameProvider.GetCommandName(CmdCode);
        }
        public string Key { get; private set; }

        public byte[] CmdCode { get; set; }
        public virtual void Execute(Protocol.Clients.PosStoreClient posStoreClient, Protocol.RequestInfos.PosStorePackageInfo package) 
        {

        }
    }
}
