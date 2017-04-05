using Pharos.MessageAgent.Data;

namespace Pharos.MessageAgent.Agent
{
    /// <summary>
    /// 消息代理服务
    /// </summary>
    public interface IMessageAgentService
    {
        /// <summary>
        /// 消息代理服务器
        /// </summary>
        MessageServer Server { get; set; }
        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="info"></param>
        /// <param name="sessionId"></param>
        void Subscribe(SubscribeInformaction info, string sessionId);
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="info"></param>
        void UnSubscribe(SubscribeInformaction info);
        /// <summary>
        /// 推送给消息队列
        /// </summary>
        /// <param name="info"></param>
        void Pubish(PubishInformaction info);
        /// <summary>
        /// 接收消息队列推送
        /// </summary>
        /// <param name="info"></param>
        void ReceiveMessage(PubishInformaction info);
    }
}
