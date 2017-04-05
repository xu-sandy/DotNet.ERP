using Pharos.Infrastructure.Data.Normalize;
using Pharos.SocketService.Retailing.Caches;
using Pharos.SocketService.Retailing.Models.Pos;
using Pharos.SocketService.Retailing.Protocol.AppServers;
using Pharos.SocketService.Retailing.Protocol.AppSessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SocketService.Retailing.Services
{
    public class SessionManager : ISessionManager
    {
        private PosStoreServer posStoreServer;
        public SessionManager(PosStoreServer posStoreServer)
        {
            this.posStoreServer = posStoreServer;

        }
        public void RegisterDeviceSession(PosStoreSessionInfo newInfo)
        {
            var cache = new SessionCache();
            var key = KeyFactory.StoreKeyFactory(newInfo.CompanyId, newInfo.StoreId);
            cache.Set(key, newInfo.SessionId);
        }

        public void NotifyAlive(PosStoreSessionInfo sInfo)
        {
            RegisterDeviceSession(sInfo);
        }

        public void ReleaseSession(PosStoreSessionInfo sInfo)
        {
            var cache = new SessionCache();
            var key = KeyFactory.StoreKeyFactory(sInfo.CompanyId, sInfo.StoreId);
            var sessionId = cache.Get(key);
            var session = posStoreServer.GetSessionByID(sessionId);
            if (session != null && session.Status != SessionStatus.Closed)
                session.Close();
            cache.Remove(key);
        }

        public PosStoreSession GetRegistered(PosStoreSessionInfo sInfo)
        {
            var cache = new SessionCache();
            var key = KeyFactory.StoreKeyFactory(sInfo.CompanyId, sInfo.StoreId);
            var sessionId = cache.Get(key);
            var session = posStoreServer.GetSessionByID(sessionId);
            return session;
        }
    }
}
