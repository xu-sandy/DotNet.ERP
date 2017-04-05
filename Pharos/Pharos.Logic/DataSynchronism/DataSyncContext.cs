using Pharos.Logic.ApiData.DataSynchronism;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Pharos.Logic.DataSynchronism
{
    public class DataSyncContext
    {
        public DataSyncContext()
        {
            Expire = 12;
        }

        public IEnumerable<TEntity> Download<TEntity>(TEntity entity, string entityName, string storeId)
            where TEntity : ISource
        {
            try
            {
                IEnumerable<TEntity> result;
                using (var db = new DataSyncDBContext())
                {
                    result = db.Database.SqlQuery<TEntity>(string.Format("exec {0} @storeId = '{1}',@expire = {2}", entityName, storeId, Expire)).ToList();
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        public ISource Instance(string entityName)
        {
            return AppDomain.CurrentDomain.CreateInstance("Pharos.Logic", "Pharos.Logic.DataSynchronism.ObjectModels." + entityName).Unwrap() as ISource;
        }

        public string DataSyncFormat(string entityName, string provider)
        {
            return string.Format("{1}s.{0}{1}", entityName, provider);
        }
        public int Expire { get; set; }


    }
}
