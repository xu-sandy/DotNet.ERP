using Pharos.SocketService.Retailing.Protocol.AppServers;
using Pharos.SocketService.Retailing.Protocol.RequestInfos;
using SuperSocket.SocketBase.Protocol;

namespace Pharos.SocketService.Retailing.Protocol.ReceiveFilters
{
    public class IReceiveFilterFactory : IReceiveFilterFactory<PosStoreRequestInfo>
    {
        public IReceiveFilter<PosStoreRequestInfo> CreateFilter(SuperSocket.SocketBase.IAppServer appServer, SuperSocket.SocketBase.IAppSession appSession, System.Net.IPEndPoint remoteEndPoint)
        {
            var server = (PosStoreServer)appServer;
            var receiveFilter = new PosStoreFixedHeaderReceiveFilter(server.CommandNameProvider);
            return receiveFilter;
        }
    }
}
