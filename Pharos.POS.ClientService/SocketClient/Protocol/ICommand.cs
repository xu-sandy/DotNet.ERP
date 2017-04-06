using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SocketClient.Retailing.Protocol
{
    public interface ICommand
    {
        string Key { get; }

        void Execute(Clients.PosStoreClient posStoreClient, RequestInfos.PosStorePackageInfo package);
    }
}
