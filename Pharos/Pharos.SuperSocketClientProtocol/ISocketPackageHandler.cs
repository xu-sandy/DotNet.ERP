using SuperSocket.ClientEngine;
using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SuperSocketClientProtocol
{
    public interface IPackageHandler
    {
        string Route { get; set; } 
    }
    public interface IPackageHandler<TClient, TPackage> : IPackageHandler
        where TClient : EasyClientBase
        where TPackage : IPackageInfo
    {
        void Handler(TClient client, TPackage package);
    }
    public interface ISocketPackageHandler : IPackageHandler<SocketClient, SockectPackageMessage>, IPackageHandler
    {

    }
}
