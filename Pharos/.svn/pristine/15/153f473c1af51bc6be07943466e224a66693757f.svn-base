using Pharos.Infrastructure.Data.Redis;
using Pharos.ObjectModels.DTOs;
using Pharos.SocketClient.Retailing.CommandProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SocketClient.Retailing.Commands
{
    public class SyncDatabaseCommand : BaseCommand
    {

        public SyncDatabaseCommand()
            : base(new byte[4] { 0x00, 0x00, 0x00, 0x05 }, new PosStoreCommandNameProvider())
        {
        }
        public override void Execute(Protocol.Clients.PosStoreClient posStoreClient, Protocol.RequestInfos.PosStorePackageInfo package)
        {
            var datas = package.Read<DatabaseChanged>();
            if (datas != null)
                RedisManager.Publish("SyncDatabase", datas.Target);
        }
    }
}
