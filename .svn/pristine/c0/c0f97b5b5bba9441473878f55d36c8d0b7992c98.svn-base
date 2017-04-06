using Pharos.Infrastructure.Data.Normalize;
using Pharos.SocketService.Retailing.Protocol;
using Pharos.SocketService.Retailing.Protocol.AppServers;
using Pharos.SocketService.Retailing.Protocol.AppSessions;
using Pharos.SocketService.Retailing.Protocol.CommandProviders;
using Pharos.SocketService.Retailing.Protocol.RequestInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SocketService.Retailing.Commands.V1
{
    public class SyncMemberNoCommand : CommandBase
    {
        public SyncMemberNoCommand()
            : base(new byte[4] { 0x00, 0x00, 0x00, 0x06 }, new PosStoreCommandNameProvider())
        {
        }

        public override void Excute(PosStoreServer server, PosStoreSession session, PosStoreRequestInfo requestInfo)
        {
            try
            {
                var datas = requestInfo.Read<MemberNoDto>();
                MemberNo memberNo = new MemberNo(datas.CompanyId, datas.StoreId);
                if (memberNo.GetNumber() <= datas.Number)
                {
                    memberNo.Reset(datas.Number);
                }
                else
                {
                    session.SendObject(this.CmdCode, new MemberNoDto() { CompanyId = memberNo.CompanyId, StoreId = memberNo.StoreId, Name = memberNo.Name, Number = memberNo.GetNumber(), SwiftNumberMode = memberNo.SwiftNumberMode });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
