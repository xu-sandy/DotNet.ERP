using Pharos.Infrastructure.Data.Normalize;
using Pharos.Infrastructure.Data.Redis;
using Pharos.SocketClient.Retailing.CommandProviders;
using Pharos.SocketClient.Retailing.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SocketClient.Retailing.Commands
{
    public class SyncSerialNumberCommand : BaseCommand
    {

        public SyncSerialNumberCommand()
            : base(new byte[4] { 0x00, 0x00, 0x00, 0x02 }, new PosStoreCommandNameProvider())
        {
        }
        public override void Execute(Protocol.Clients.PosStoreClient posStoreClient, Protocol.RequestInfos.PosStorePackageInfo package)
        {
            var datas = package.Read<PaySnDto>();
            PaySn customOrderSn = new PaySn(datas.CompanyId, datas.StoreId, datas.MachineSn);
            if (customOrderSn.GetNumber() <= datas.Number)
            {
                customOrderSn.ResetSerialNumber(datas.Number);
            }
            else
            {
                posStoreClient.SendObject(this.CmdCode, new PaySnDto() { CompanyId = customOrderSn.CompanyId, MachineSn = customOrderSn.MachineSn, StoreId = customOrderSn.StoreId, Name = customOrderSn.Name, Number = customOrderSn.GetNumber(), SwiftNumberMode = customOrderSn.SwiftNumberMode });
            }
        }
    }
}
