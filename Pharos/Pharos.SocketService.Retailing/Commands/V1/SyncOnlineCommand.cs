﻿using Pharos.Infrastructure.Data.Normalize;
using Pharos.Logic.ApiData.Pos;
using Pharos.Logic.ApiData.Pos.Cache;
using Pharos.SocketService.Retailing.Protocol;
using Pharos.SocketService.Retailing.Protocol.AppServers;
using Pharos.SocketService.Retailing.Protocol.AppSessions;
using Pharos.SocketService.Retailing.Protocol.CommandProviders;
using Pharos.SocketService.Retailing.Protocol.RequestInfos;
using System;

namespace Pharos.SocketService.Retailing.Commands.V1
{
    public class SyncOnlineCommand : CommandBase
    {
        public SyncOnlineCommand()
            : base(new byte[4] { 0x00, 0x00, 0x00, 0x04 }, new PosStoreCommandNameProvider())
        {
        }

        public override void Excute(PosStoreServer server, PosStoreSession session, PosStoreRequestInfo requestInfo)
        {
            try
            {
                var datas = requestInfo.Read<MachineInformation>();
                if (datas != null)
                {
                    var cache = new OnlineCache();
                    var key = KeyFactory.MachineKeyFactory(datas.CompanyId, datas.StoreId, datas.MachineSn, datas.DeviceSn);
                    cache.Set(key, datas);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
