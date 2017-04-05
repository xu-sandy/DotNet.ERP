﻿using Pharos.SuperSocketProtocol;
using System;
using Pharos.MessageAgent.Data;

namespace Pharos.MessageAgent.Commands
{
    public class UnSubscribeCommad : CommandBase
    {
        public UnSubscribeCommad() : base(0x01, 0x00, 0x00, 0x03) { }
        public override void Execute(SocketServer server, SocketSession session, SockectRequestMessage requestInfo)
        {
            try
            {
                SubscribeInformaction info;
                if (requestInfo.TryReadFromJsonStream<SubscribeInformaction>(out info))
                {
                    var msgServer = (MessageServer)server;
                    msgServer.MessageAgent.UnSubscribe(info);
                }
            }
            catch (Exception ex)
            {
                session.Logger.Error(string.Format("取消订阅失败，请查看订阅信息是否完整！异常消息：{0}", ex.Message), ex);
            }
        }
    }
}
