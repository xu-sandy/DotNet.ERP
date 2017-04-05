using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Pharos.Wpf.Extensions
{
    public static class CollectionsExtension
    {
        public static List<Type> InitListType(this List<Type> _this, IEnumerable<XElement> ignoreNodes)
        {
            if (_this == null)
                _this = new List<Type>();
            foreach (var item in ignoreNodes)
            {
                var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
                foreach (var ass in assemblies)
                {
                    var type = ass.GetTypes().FirstOrDefault(o => o.ToString() == item.Value.Trim());
                    if (type != null)
                    {
                        _this.Add(type);
                        break;
                    }
                }
            }
            return _this;
        }
    }
}
