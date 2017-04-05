﻿using Pharos.Infrastructure.Data.Cache.Interface;
using Pharos.Infrastructure.Data.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Infrastructure.Data.Cache
{
    /// <summary>
    /// 应用程序域缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AppDomainCacheWrapper<T> : ICache<T>
    {
        public AppDomainCacheWrapper()
            : this(typeof(T).Name)
        {

        }
        public AppDomainCacheWrapper(string name)
        {
            Name = name;
        }


        public string Name { get; private set; }

        public bool ContainsKey(string key)
        {
            try
            {
                var data = AppDomain.CurrentDomain.GetData(key + Name);
                return data != null && data is T;
            }
            catch
            {
                return false;
            }
        }
        public T Get(string key)
        {
            var data = AppDomain.CurrentDomain.GetData(key + Name);
            if (data != null && data is T)
            {
                return (T)data;
            }
            return default(T);
        }

        public void Remove(string key)
        {
            AppDomain.CurrentDomain.SetData(key + Name, null);
        }

        public void Set(string key, T value)
        {
            AppDomain.CurrentDomain.SetData(key + Name, value);
        }
    }
}
