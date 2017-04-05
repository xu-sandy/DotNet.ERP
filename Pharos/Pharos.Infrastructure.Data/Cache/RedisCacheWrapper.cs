﻿using Newtonsoft.Json;
using Pharos.Infrastructure.Data.Cache.Interface;
using Pharos.Infrastructure.Data.Redis;
using System;

namespace Pharos.Infrastructure.Data.Cache
{
    public class RedisCacheWrapper<T> : IRedisCacheWrapper<T>
    {
        #region Fields
        /// <summary>
        /// 是否忽略大小写
        /// </summary>
        bool _ignoreCase = false;
        #endregion Fields

        #region constructor
        /// <summary>
        /// 初始化RedisCache非过期性存储
        /// </summary>
        /// <param name="name">缓存名称</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        public RedisCacheWrapper(string name, bool ignoreCase = false)
            : this(name, default(TimeSpan), ignoreCase)
        {
        }
        /// <summary>
        /// 初始化RedisCache，定时过期性存储
        /// </summary>
        /// <param name="name">缓存名称</param>
        /// <param name="slidingExpiration">缓存过期时间</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        public RedisCacheWrapper(string name, TimeSpan slidingExpiration, bool ignoreCase = false)
        {
            Name = name;
            SlidingExpiration = slidingExpiration;
            _ignoreCase = ignoreCase;
            JsonSerializerSettings = new JsonSerializerSettings();
        }
        #endregion constructor

        #region Property
        /// <summary>
        /// 缓存统一失效时间
        /// </summary>
        public TimeSpan SlidingExpiration { get; set; }

        public JsonSerializerSettings JsonSerializerSettings { get; set; }
        /// <summary>
        /// 缓存实例名称
        /// </summary>
        public string Name { get; private set; }
        #endregion Property

        #region Method
        /// <summary>
        /// 是否包含指定的Key
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否存在</returns>
        public virtual bool ContainsKey(string key)
        {
            var redisClient = RedisManager.Connection;
            return redisClient.GetDatabase().KeyExists(key + Name);
        }
        /// <summary>
        /// 获取指定Key的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T Get(string key)
        {
            var redisClient = RedisManager.Connection;
            var json = redisClient.GetDatabase().StringGet(key + Name);
            if (!string.IsNullOrEmpty(json))
                return JsonConvert.DeserializeObject<T>(json, JsonSerializerSettings);
            return default(T);
        }
        /// <summary>
        /// 移除key所对应的值
        /// </summary>
        /// <param name="key"></param>
        public virtual void Remove(string key)
        {
            var redisClient = RedisManager.Connection;
            redisClient.GetDatabase().KeyDelete(key + Name);
        }
        /// <summary>
        /// 设置键值缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public virtual void Set(string key, T value)
        {
            var redisClient = RedisManager.Connection;
            var json = JsonConvert.SerializeObject(value);
            if (SlidingExpiration != default(TimeSpan))
            {
                if (!redisClient.GetDatabase().StringSet(key + Name, json, SlidingExpiration))
                {
                    throw new Exception("缓存设置失败！");
                }
            }
            else 
            {
                if (!redisClient.GetDatabase().StringSet(key + Name, json)) 
                {
                    throw new Exception("缓存设置失败！");
                }
            }
        }


        #endregion Method
    }
}
