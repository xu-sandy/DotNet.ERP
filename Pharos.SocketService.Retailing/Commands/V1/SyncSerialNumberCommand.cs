using Pharos.Infrastructure.Data.Normalize;
using Pharos.SocketService.Retailing.Protocol;
using Pharos.SocketService.Retailing.Protocol.AppServers;
using Pharos.SocketService.Retailing.Protocol.AppSessions;
using Pharos.SocketService.Retailing.Protocol.CommandProviders;
using Pharos.SocketService.Retailing.Protocol.RequestInfos;
using System;

namespace Pharos.SocketService.Retailing.Commands.V1
{
    public class SyncSerialNumberCommand : CommandBase
    {
        public SyncSerialNumberCommand()
            : base(new byte[4] { 0x00, 0x00, 0x00, 0x02 }, new PosStoreCommandNameProvider())
        {
        }

        public override void Excute(PosStoreServer server, PosStoreSession session, PosStoreRequestInfo requestInfo)
        {
            try
            {
                var datas = requestInfo.Read<PaySnDto>();
                PaySn customOrderSn = new PaySn(datas.CompanyId, datas.StoreId, datas.MachineSn);
                if (customOrderSn.GetNumber() <= datas.Number)
                {
                    customOrderSn.ResetSerialNumber(datas.Number);
                }
                else
                {
                    session.SendObject(this.CmdCode, new PaySnDto() { CompanyId = customOrderSn.CompanyId, MachineSn = customOrderSn.MachineSn, StoreId = customOrderSn.StoreId, Name = customOrderSn.Name, Number = customOrderSn.GetNumber(), SwiftNumberMode = customOrderSn.SwiftNumberMode });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
