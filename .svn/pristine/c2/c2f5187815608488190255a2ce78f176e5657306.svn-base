using Pharos.Infrastructure.Data.Normalize;
using Pharos.Logic.ApiData.Pos;
using Pharos.Logic.ApiData.Pos.Cache;
using Pharos.SocketClient.Retailing.CommandProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SocketClient.Retailing.Commands
{
    public class SyncOnlineCommand : BaseCommand
    {

        public SyncOnlineCommand()
            : base(new byte[4] { 0x00, 0x00, 0x00, 0x04 }, new PosStoreCommandNameProvider())
        {
        }
        public override void Execute(Protocol.Clients.PosStoreClient posStoreClient, Protocol.RequestInfos.PosStorePackageInfo package)
        {
            var datas = package.Read<MachineInformation>();
            if (datas != null)
            {
                var cache = new OnlineCache();
                var key = KeyFactory.MachineKeyFactory(datas.CompanyToken, datas.StoreId, datas.MachineSn, datas.DeviceSn);
                cache.Set(key, datas);
            }
        }
    }
}
