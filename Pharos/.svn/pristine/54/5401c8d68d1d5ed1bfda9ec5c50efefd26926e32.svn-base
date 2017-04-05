using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Pharos.Logic.Entity
{
    public static class BaseEntityExtension
    {
        public static Server InitEntity<Local, Server>(Local tLocal)
            where Server : new()
        {
            var _this = new Server();
            var type = tLocal.GetType();
            var entityType = _this.GetType();
            var properties = type.GetProperties();
            foreach (PropertyInfo info in properties)
            {
                var value = info.GetValue(tLocal, null);
                var entityProperty = entityType.GetProperty(info.Name);
                if (entityProperty != null && entityProperty.GetSetMethod() != null)
                    entityProperty.SetValue(_this, value, null);
            }
            return _this;
        }

        public static Server InitEntity<Server>(object tLocal)
          where Server : new()
        {
            var _this = new Server();
            var type = tLocal.GetType();
            var entityType = _this.GetType();
            var properties = type.GetProperties();
            foreach (PropertyInfo info in properties)
            {
                var value = info.GetValue(tLocal, null);
                var entityProperty = entityType.GetProperty(info.Name);
                if (entityProperty != null && entityProperty.GetSetMethod() != null)
                    entityProperty.SetValue(_this, value, null);
            }
            return _this;
        }
    }
}
