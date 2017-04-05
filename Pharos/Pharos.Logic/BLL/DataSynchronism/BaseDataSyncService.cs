using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Pharos.Logic.BLL.DataSynchronism
{
    public abstract class BaseDataSyncService<TEntity, TDtos> : BaseService<TEntity>, IDataSyncService<TEntity>
        where TEntity : class, new()
        where TDtos : class, new()
    {
        public UpdateFormData UpdateFormDatas { get; set; }
        public virtual bool UpLoad(IEnumerable<TEntity> datas, string storeId)
        {
            var serverRepository = CurrentRepository;
            if (datas == null)
            {
                return false;
            }
            var tempDatas = datas.Select(o => Pharos.Logic.Entity.BaseEntityExtension.InitEntity<TEntity>(o));
            serverRepository.AddRange(tempDatas.ToList());
            return true;
        }

        private IEnumerable<TEntity> ConvertToEntity(string json)
        {
            return JsonConvert.DeserializeObject<IEnumerable<TEntity>>(json);
        }

        public virtual IEnumerable<TEntity> Download(string storeId, string entityType)
        {
            BaseDataSyncService<TEntity, TDtos>.IsForcedExpired = true;
            var serverRepository = CurrentRepository;
            var sources = serverRepository.FindList(o => true).ToList();
            return sources;
        }


        IEnumerable<dynamic> IDataSyncService.Download(string storeId, string entityType)
        {
            var result = Download(storeId, entityType);
            return result.Select(o => Pharos.Logic.Entity.BaseEntityExtension.InitEntity<TDtos>(o));
        }


        public virtual bool Update(string key, string json, string storeId)
        {
            return false;
        }


        public virtual IEnumerable<dynamic> Export(string storeId, string entityType)
        {
            return Download(storeId, entityType);
        }

        public virtual bool UpLoad(string key, string json, string storeId, UpdateFormData datas)
        {
            UpdateFormDatas = datas;
            return UpLoad(ConvertToEntity(json), storeId);
        }

        public virtual bool Update(string key, string json, string storeId, UpdateFormData datas)
        {
            UpdateFormDatas = datas;
            return Update(key, json, storeId);
        }
    }


}
