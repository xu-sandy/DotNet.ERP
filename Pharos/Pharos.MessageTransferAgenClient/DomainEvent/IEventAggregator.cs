using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.MessageTransferAgenClient.DomainEvent
{
    public interface IEventAggregator
    {
        void Subscribe(string topic, IEventHandler handler);
        void Subscribe<TEvent>(string topic, Action<TEvent> handler) where TEvent : class, IEvent;
        Task SubscribeAsync(string topic, IEventHandler handler);
        void Publish<TEvent>(string topic, TEvent domainEvent) where TEvent : class, IEvent;
        Task PublishAsync<TEvent>(string topic, TEvent domainEvent) where TEvent : class, IEvent;
        void Unsubscribe(string topic, IEventHandler handler = null);
        void Unsubscribe<TEvent>(string topic, Action<TEvent> handler = null) where TEvent : class, IEvent;
        Task UnsubscribeAsync(string topic, IEventHandler handler = null);
        void UnsubscribeAll();
        Task UnsubscribeAllAsync();
    }
}
