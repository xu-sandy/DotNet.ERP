using Pharos.SocketService.Retailing.Protocol.CommandProviders;
using Pharos.SocketService.Retailing.Protocol.RequestInfos;

namespace Pharos.SocketService.Retailing.Protocol.ReceiveFilters
{
    public class PosStoreFixedHeaderReceiveFilter : CustomFixedHeaderReceiveFilter<PosStoreRequestInfo>
    {
        ICommandNameProvider commandNameProvider;
        public PosStoreFixedHeaderReceiveFilter(ICommandNameProvider commandNameProvider)
        {
            this.commandNameProvider = commandNameProvider;
        }
        public override PosStoreRequestInfo LoadRequestInfo(byte[] cmdCode, byte[] bodyBuffer)
        {
            var result = new PosStoreRequestInfo(commandNameProvider.GetCommandName(cmdCode), bodyBuffer);
            return result;
        }
    }
}
