﻿using StackExchange.Redis;
using System;
using System.IO;
using System.Linq;
using Pharos.Utility.Helpers;
namespace Pharos.Infrastructure.Data.Redis
{
    public class RedisManager
    {
        /// <summary>
        /// redis配置文件信息
        /// </summary>
        private static RedisConfiguration redisConfigInfo = RedisConfiguration.GetConfig();


        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(redisConfigInfo.ConnectionString);
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        public static void Subscribe(string title, Action<RedisChannel, string> handler)
        {
            ISubscriber sub = Connection.GetSubscriber();
            sub.Subscribe(title, (channel, msg) => { handler(channel, (string)msg); });
        }


        public static void UnSubscribe(string title)
        {
            ISubscriber sub = Connection.GetSubscriber();
            sub.Unsubscribe(new RedisChannel(title, RedisChannel.PatternMode.Auto));
        }
        public static void Publish(string title, string info)
        {
            ISubscriber sub = Connection.GetSubscriber();
            sub.Publish(title, info);
        }
        public static void Publish<T>(string title, T info)
        {
            Publish(title, info.ToJson());
        }
    }
}
