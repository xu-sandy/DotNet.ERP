using Pharos.Infrastructure.Data.Normalize;
using Pharos.SocketClient.Retailing.CommandProviders;
using Pharos.SocketClient.Retailing.Commands;
using Pharos.SocketClient.Retailing.Protocol.Clients;
using Pharos.SocketClient.Retailing.Protocol.RequestInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.SocketClient.Retailing.Commands
{
    public class SyncMemberNoCommand : BaseCommand
    {

        public SyncMemberNoCommand()
            : base(new byte[4] { 0x00, 0x00, 0x00, 0x06 }, new PosStoreCommandNameProvider())
        {
        }
        public override void Execute(PosStoreClient posStoreClient, PosStorePackageInfo package)
        {
            var datas = package.Read<MemberNoDto>();
            MemberNo memberNo = new MemberNo(datas.CompanyId, datas.StoreId);
            if (memberNo.GetNumber() <= datas.Number)
            {
                memberNo.Reset(datas.Number);
            }
            else
            {
                posStoreClient.SendObject(this.CmdCode, new MemberNoDto() { CompanyId = memberNo.CompanyId, StoreId = memberNo.StoreId, Name = memberNo.Name, Number = memberNo.GetNumber(), SwiftNumberMode = memberNo.SwiftNumberMode });
            }
        }
    }
}
