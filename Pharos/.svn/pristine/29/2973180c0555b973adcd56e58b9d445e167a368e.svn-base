using Newtonsoft.Json;
using Pharos.MessageAgent.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.MessageTransferAgenClient.DomainEvent
{
    /// <summary>
    /// 表示实现该接口的类型为事件处理器。
    /// </summary>
    /// <typeparam name="TEvent">事件的类型。</typeparam>
    public abstract class BaseEventHandler<TEvent> : IEventHandler
        where TEvent : IEvent
    {
        public BaseEventHandler(string topic)
        {
            Topic = topic;
        }
        public string Topic { get; set; }

        #region Methods
        /// <summary>
        /// 处理给定的事件。
        /// </summary>
        /// <param name="evnt">需要处理的事件。</param>
        public abstract void Handle(TEvent evnt);
        public void Handle(PubishInformaction evnt)
        {
            var eventArgs = JsonConvert.DeserializeObject<TEvent>(evnt.Content);
            Handle(eventArgs);
        }
        #endregion


    }
    public interface IEventHandler
    {
        string Topic { get; set; }
        /// <summary>
        /// 处理给定的事件。
        /// </summary>
        /// <param name="evnt">需要处理的事件。</param>
        void Handle(PubishInformaction evnt);
    }
}
