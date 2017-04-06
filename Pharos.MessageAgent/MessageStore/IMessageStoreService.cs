using Pharos.MessageAgent.Data;
using System.Collections.Generic;

namespace Pharos.MessageAgent.Agent
{
    /// <summary>
    /// 消息存储服务（注意：根据存储服务，实现锁（读写锁或者互斥锁，视情况而定））
    /// </summary>
    public interface IMessageStoreService
    {
        /// <summary>
        /// 获取并移除一条失败推送,当未能找到任何失败推送信息时，返回null
        /// </summary>
        /// <param name="subscribeId"></param>
        /// <param name="topic"></param>
        /// <returns></returns>
        PubishInformaction GetAndRemoveFailureRecord(string subscribeId, string topic);

        /// <summary>
        /// 保存推送失败记录(需要根据GetAndRemoveFailureRecord方法取值是否方便快速，进行存储结构及存储介质的选择)
        /// </summary>
        /// <param name="pubish"></param>
        /// <param name="subscribes"></param>
        void SaveFailureRecords(PubishInformaction pubish, IEnumerable<MessageSubscribeRecord> subscribes);

        /// <summary>
        /// 根据订阅源ID与主题，获取订阅信息
        /// </summary>
        /// <param name="subscribeId"></param>
        /// <param name="topic"></param>
        /// <returns>未知订阅信息返回null</returns>
        MessageSubscribeRecord GetSubscribe(string subscribeId, string topic);
        /// <summary>
        /// 保存订阅记录
        /// </summary>
        /// <param name="record"></param>
        void SaveSubscribe(MessageSubscribeRecord record);
        /// <summary>
        /// 更新订阅记录的SessionID
        /// </summary>
        /// <param name="socketSessionId"></param>
        void UpdateSubscribe(MessageSubscribeRecord record);
        /// <summary>
        /// 移除订阅记录
        /// </summary>
        /// <param name="subscribeId"></param>
        /// <param name="topic"></param>
        void RemoveSubscribe(string subscribeId, string topic);
        /// <summary>
        /// 是否存在查询主题的订阅
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        bool HasTopicSubscribe(string topic);
        /// <summary>
        /// 根据主题获取订阅记录【注意：请在初始化器中以Dictionary<string, List<MessageSubscribeRecord>> 的形式缓存一份副本，改方法之间返回副本的结果，以加快查询速度】
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        IEnumerable<MessageSubscribeRecord> GetSubscribes(string topic);
    }
}
