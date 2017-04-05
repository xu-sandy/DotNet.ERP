﻿using Pharos.MessageAgent.Data;
using Pharos.SuperSocketProtocol;
using System;

namespace Pharos.MessageAgent.Commands
{
    public class PubishCommand : CommandBase
    {
        public PubishCommand() : base(0x01, 0x00, 0x00, 0x02) { }

        public override void Execute(SocketServer server, SocketSession session, SockectRequestMessage requestInfo)
        {
            PubishInformaction info = null;
            try
            {
                if (TryReadPubishInformaction(requestInfo, out info))
                {
                    var msgServer = (MessageServer)server;
                    msgServer.MessageAgent.Pubish(info);
                }
                else
                {
                    throw new MessageAgentException("未能正常解析推送数据！");
                }
            }
            catch (Exception ex)
            {
                if (info != null)
                {
                    session.Logger.Error(string.Format("推送失败，请查看推送信息是否完整！异常消息：{0}{1}推送主题：{2}推送内容：{3}", ex.Message, Environment.NewLine, info.Topic, info.Content), ex);
                }
                else
                {
                    session.Logger.Error(string.Format("推送失败，请查看推送信息是否完整！异常消息：{0}{1}推送信息为空！", ex.Message, Environment.NewLine), ex);
                }
            }
        }

        private bool TryReadPubishInformaction(SockectRequestMessage requestInfo, out PubishInformaction info)
        {
            info = null;
            if (requestInfo.Body.Length < 5) return false;

            var len = BitConverter.ToInt32(requestInfo.Body, 0);
            string topic;
            if (requestInfo.TryReadFromText(requestInfo.Body, 4, len, out topic))
            {
                string content;
                if (requestInfo.TryReadFromText(requestInfo.Body, 4 + len, requestInfo.Body.Length - 4 - len, out content))
                {
                    info = new PubishInformaction()
                    {
                        Topic = topic,
                        Content = content
                    };
                    return true;
                }
            }
            return false;
        }
    }
}
