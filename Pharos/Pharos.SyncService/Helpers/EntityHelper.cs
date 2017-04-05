﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SyncService.Helpers
{
    public static class EntityHelper
    {
        public static T InitEntity<T>(this T _entity, object data, bool initSyncItemVersion = true)
        {
            if (_entity == null)
                throw new StackOverflowException("初始化对象entity不能为空！");
            if (data == null)
                throw new StackOverflowException("初始化对象data不能为空！");

            var type = typeof(T);
            var dataType = data.GetType();
            var properties = type.GetProperties();
            var dataProperties = dataType.GetProperties().ToList();
            foreach (var item in properties)
            {
                if (!initSyncItemVersion && item.Name == "SyncItemVersion")
                {
                    continue;
                }
                var dataProperty = dataProperties.FirstOrDefault(o => o.Name == item.Name && o.CanRead);
                if (item.CanWrite && dataProperty != null)
                    item.SetValue(_entity, dataProperty.GetValue(data, null), null);
            }
            return _entity;
        }

        public static string SyncIdsToString(this IEnumerable<Guid> ids)
        {
            var result = string.Empty;
            foreach (var item in ids)
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result += ",";
                }
                result += item.ToString();
            }
            return result;
        }
        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            var index = 0;
            foreach (var item in source)
            {
                action(item, index);
                index++;
            }
        }
    }

}
