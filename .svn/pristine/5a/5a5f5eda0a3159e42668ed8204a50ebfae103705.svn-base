using Pharos.Logic.LocalEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.DataSynchronism
{
    public interface IDataSyncService
    {
        bool UpLoad(string key, string json, string storeId,UpdateFormData datas);
        bool Update(string key, string json, string storeId, UpdateFormData datas);
        IEnumerable<dynamic> Download(string storeId, string entityType);//pos端导出
        IEnumerable<dynamic> Export(string storeId, string entityType);//服务端导出
    }
    public interface IDataSyncService<TEntity> : IDataSyncService
        where TEntity : class,new()
    {
       new IEnumerable<TEntity> Download(string storeId, string entityType);

    }
}
