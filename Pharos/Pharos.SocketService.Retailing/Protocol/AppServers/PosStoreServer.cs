﻿using Pharos.SocketService.Retailing.Protocol.AppSessions;
using Pharos.SocketService.Retailing.Protocol.CommandProviders;
using Pharos.SocketService.Retailing.Protocol.ReceiveFilters;
using Pharos.SocketService.Retailing.Protocol.RequestInfos;
using Pharos.SocketService.Retailing.Services;
using SuperSocket.SocketBase;
using Pharos.Infrastructure.Data.Redis;
using Newtonsoft.Json;
using Pharos.Infrastructure.Data.Normalize;
using Pharos.ObjectModels.DTOs;
using Pharos.Logic.ApiData.Pos;
using Pharos.Logic.ApiData.Pos.Sale.Suspend;
using Pharos.Logic.ApiData.Pos.Sale;

namespace Pharos.SocketService.Retailing.Protocol.AppServers
{
    public class PosStoreServer : AppServer<PosStoreSession, PosStoreRequestInfo>
    {
        public PosStoreServer()
            : base(new IReceiveFilterFactory())
        {
            _cmdNameProvider = new PosStoreCommandNameProvider();
            _sessionManager = new SessionManager(this);
            SubscribeStoreMesssage();
        }

        private ICommandNameProvider _cmdNameProvider = null;

        public ICommandNameProvider CommandNameProvider
        {
            get { return _cmdNameProvider; }
        }
        private ISessionManager _sessionManager = null;

        internal ISessionManager SessionManager
        {
            get { return _sessionManager; }
        }

        private void SubscribeStoreMesssage()
        {
            RedisManager.Subscribe("SyncDatabase", (channel, info) =>
            {
                var databaseChanged = JsonConvert.DeserializeObject<DatabaseChanged>(info);
                var storeIds = databaseChanged.StoreId.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
                foreach (var storeId in storeIds)
                {
                    var session = SessionManager.GetRegistered(new Models.Pos.PosStoreSessionInfo() { CompanyId = databaseChanged.CompanyId, StoreId = storeId });
                    if (session != null && session.Status == SessionStatus.Started)
                    {
                        session.SendObject(new byte[4] { 0x00, 0x00, 0x00, 0x05 }, databaseChanged);
                    }
                }
            });
            RedisManager.Subscribe("SyncSerialNumber", (channel, info) =>
            {
                var paysn = JsonConvert.DeserializeObject<PaySnDto>(info);
                var session = SessionManager.GetRegistered(new Models.Pos.PosStoreSessionInfo() { CompanyId = paysn.CompanyId, StoreId = paysn.StoreId });
                if (session != null && session.Status == SessionStatus.Started)
                {
                    session.SendObject(new byte[4] { 0x00, 0x00, 0x00, 0x02 }, paysn);
                }
            });
            RedisManager.Subscribe("MemberNo", (channel, info) =>
            {
                var memberNoDto = JsonConvert.DeserializeObject<MemberNoDto>(info);
                var session = SessionManager.GetRegistered(new Models.Pos.PosStoreSessionInfo() { CompanyId = memberNoDto.CompanyId, StoreId = memberNoDto.StoreId });
                if (session != null && session.Status == SessionStatus.Started)
                {
                    session.SendObject(new byte[4] { 0x00, 0x00, 0x00, 0x06 }, memberNoDto);
                }
            });
            RedisManager.Subscribe("SyncOnlineCache", (channel, info) =>
            {
                var machineInformation = JsonConvert.DeserializeObject<MachineInformation>(info);
                var session = SessionManager.GetRegistered(new Models.Pos.PosStoreSessionInfo() { CompanyId = machineInformation.CompanyId, StoreId = machineInformation.StoreId });
                if (session != null && session.Status == SessionStatus.Started)
                {
                    session.SendObject(new byte[4] { 0x00, 0x00, 0x00, 0x04 }, machineInformation);
                }

            });
            //RedisManager.Subscribe("SyncShoppingCartCache", (channel, info) =>
            //{
            //    var jsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings();
            //    jsonSerializerSettings.Converters.Add(new BarcodeConverter());
            //    var shoppingCart = JsonConvert.DeserializeObject<ShoppingCart>(info, jsonSerializerSettings);
            //    var session = SessionManager.GetRegistered(new Models.Pos.PosStoreSessionInfo() { CompanyId = shoppingCart.MachineInformation.CompanyId, StoreId = shoppingCart.MachineInformation.StoreId });
            //    if (session != null && session.Status == SessionStatus.Started)
            //    {
            //        session.SendObject(new byte[4] { 0x00, 0x00, 0x00, 0x03 }, shoppingCart);
            //    }
            //});
        }
    }
}
