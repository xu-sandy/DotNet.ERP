﻿
namespace Pharos.MessageAgent.Data
{
    /// <summary>
    /// 订阅记录信息
    /// </summary>
    public class MessageSubscribeRecord
    {
        /// <summary>
        /// 当前会话ID
        /// </summary>
        public string CurrentSessionId { get; set; }
        /// <summary>
        /// 订阅信息
        /// </summary>
        public SubscribeInformaction SubscribeInformaction { get; set; }
    }
}
