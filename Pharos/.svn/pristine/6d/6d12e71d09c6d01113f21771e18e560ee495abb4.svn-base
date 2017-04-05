using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.SyncService.Helpers;
using Pharos.SyncService.Exceptions;

namespace Pharos.SyncService.SyncEntities
{
    [Serializable]
    public class Package : SyncDataObject
    {
        public Package()
        {
            EnityCollectionsDict = new Dictionary<string, List<ISyncDataObject>>();
        }

        public void Init<T>(IEnumerable<T> entities)
            where T : ISyncDataObject
        {
            if (entities == null)
                throw new SyncException("实体集合不能为NULL！");
            foreach (var item in entities)
            {
                var key = typeof(T).ToString();
                if (!EnityCollectionsDict.ContainsKey(key))
                {
                    EnityCollectionsDict[key] = new List<ISyncDataObject>();
                }
                EnityCollectionsDict[key].Add(item);
            }
        }

        public IEnumerable<T> GetEntities<T>()
            where T : new()
        {
            string key = typeof(T).ToString();
            if (EnityCollectionsDict.ContainsKey(key) && EnityCollectionsDict[key] != null)
            {
                return EnityCollectionsDict[key].Select(o => new T().InitEntity<T>(o)).ToList();
            }
            else
            {
                return new List<T>();
            }
        }
        public IDictionary<string, List<ISyncDataObject>> EnityCollectionsDict { get; set; }
    }
}
