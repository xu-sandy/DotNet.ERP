using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Pharos.Logic.LocalEntity
{
    public abstract class BaseEntity
    {
        public virtual Local InitEntity<Local, Server>(Server tServer)
         where Local : BaseEntity
        {
            var type = tServer.GetType();
            var entityType = this.GetType();
            var properties = type.GetProperties();
            foreach (PropertyInfo info in properties)
            {
                var value = info.GetValue(tServer, null);
                PropertyInfo entityProperty = null;
                if (info.Name == "Id" && info.PropertyType == typeof(Int64))
                {
                    continue;
                }
                else
                {
                    entityProperty = entityType.GetProperty(info.Name);
                }
                if (entityProperty != null && entityProperty.GetSetMethod() != null)
                    entityProperty.SetValue(this, value, null);
            }
            return this as Local;
        }

        public virtual bool EqualsEntityPrimaryKey<TEntity>(TEntity entity)
        {
            var currentType = this.GetType();
            var entityType = entity.GetType();
            if (currentType != entityType)
            {
                return false;
            }
            var properties = currentType.GetProperties();
            var property = properties.Where(o => o.GetCustomAttributes(typeof(LocalKeyAttribute), true).Length == 1);
            if (property == null || property.Count() == 0)
            {
                return false;
            }

            if (property.Any(o => !o.GetValue(this, null).Equals(o.GetValue(entity, null))))
            {
                return false;
            }
            return true;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool EqualsEntityAndUpdateValue<TEntity>(TEntity entity)
           where TEntity : BaseEntity
        {

            var _thisType = this.GetType();
            var entityType = entity.GetType();
            if (_thisType != entityType)
            {
                throw new Exception("类型错误！");
            }
            var properties = _thisType.GetProperties();
            foreach (PropertyInfo item in properties)
            {
                if (item.Name == "Id") continue;
                var value1 = item.GetValue(this, null);
                var value2 = item.GetValue(entity, null);
                var hasSet = item.GetSetMethod();
                if (value1 != value2)
                {
                    if (value1 != null && value2 != null)
                    {
                        if (!value2.Equals(value1))
                        {
                            if (hasSet != null)
                            {
                                item.SetValue(this, value2, null);
                            }
                            return false;
                        }
                    }
                    else if (value1 == null && value2 != null || value1 != null && value2 == null)
                    {
                        if (hasSet != null)
                        {
                            item.SetValue(this, value2, null);
                        }
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
