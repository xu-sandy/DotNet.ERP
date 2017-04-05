using Pharos.MessageAgent.Data;
using Pharos.MessageAgent.Extensions;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Pharos.MessageAgent.MessageQueues.Redis
{
    public class RedisMessageQueue : IMessageQueue
    {
        public readonly Action<RedisChannel, RedisValue> Handler;
        public RedisMessageQueue()
        {
            Server = MessageServer.Current;
            Handler = ((channel, msg) =>
            {
                Task.Factory.StartNew((o) =>
                {
                    Server.MessageAgent.ReceiveMessage(o as PubishInformaction);
                }, new PubishInformaction() { Topic = channel, Content = msg });
            });
        }
        public MessageServer Server { get; set; }

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return MessageServerConfiguration.GetConfig().MessageQueueConnectionString.GetConnection();
            }
        }

        public void Pubish(PubishInformaction info)
        {
            ISubscriber sub = Connection.GetSubscriber();
            sub.Publish(info.Topic, info.Content);
        }

        public void Subscribe(SubscribeInformaction info)
        {

            ISubscriber sub = Connection.GetSubscriber();
            sub.Subscribe(info.Topic, Handler);
        }


        public void UnSubscribe(SubscribeInformaction info)
        {
            ISubscriber sub = Connection.GetSubscriber();
            sub.Unsubscribe(info.Topic, Handler);
        }

    }
}
