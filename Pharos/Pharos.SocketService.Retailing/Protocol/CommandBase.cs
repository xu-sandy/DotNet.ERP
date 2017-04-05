using Pharos.SocketService.Retailing.Protocol.AppServers;
using Pharos.SocketService.Retailing.Protocol.AppSessions;
using Pharos.SocketService.Retailing.Protocol.CommandProviders;
using Pharos.SocketService.Retailing.Protocol.RequestInfos;
using SuperSocket.SocketBase.Command;

namespace Pharos.SocketService.Retailing.Protocol
{
    public abstract class CommandBase : ICommand<PosStoreSession, PosStoreRequestInfo>
    {
        ICommandNameProvider commandNameProvider;
        public CommandBase(byte[] cmdCode, ICommandNameProvider commandNameProvider)
        {
            CmdCode = cmdCode;
            this.commandNameProvider = commandNameProvider;
            Name = commandNameProvider.GetCommandName(cmdCode);
        }
        public void ExecuteCommand(PosStoreSession session, PosStoreRequestInfo requestInfo)
        {
            Excute(session.PosStoreServer, session, requestInfo);
        }
        public abstract void Excute(PosStoreServer server, PosStoreSession session, PosStoreRequestInfo requestInfo);

        public byte[] CmdCode { get; private set; }
        public string Name { get; private set; }
    }
}
