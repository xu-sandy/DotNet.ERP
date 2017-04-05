using Pharos.SocketService.Retailing.Models.Pos;
using Pharos.SocketService.Retailing.Protocol.AppSessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SocketService.Retailing.Services
{
    public interface ISessionManager
    {
        /// <summary>
        /// 登记一个会话
        /// </summary>
        /// <param name="newInfo">会话信息描述</param>
        void RegisterDeviceSession(PosStoreSessionInfo newInfo);
        /// <summary>
        /// 宣告会话的存活，以刷新其在内存缓存中的激活时间，避免被逐出
        /// </summary>
        /// <param name="sInfo">会话信息描述</param>
        void NotifyAlive(PosStoreSessionInfo sInfo);

        /// <summary>
        /// 释放一个会话，立即将会话逐出缓存并断开
        /// </summary>
        /// <param name="sInfo">会话信息描述</param>
        void ReleaseSession(PosStoreSessionInfo sInfo);

        /// <summary>
        /// 使用idCode获取已登记会话，实际上设备会话缓存以idCode为键，设备会话信息为值进行缓存
        /// </summary>
        /// <param name="sInfo">会话信息描述</param>
        /// <returns>会话信息，如果缓存中不存在则返回null</returns>
        PosStoreSession GetRegistered(PosStoreSessionInfo sInfo);
    }
}
