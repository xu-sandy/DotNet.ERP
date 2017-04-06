using Newtonsoft.Json;
using Pharos.Logic.BLL.DataSynchronism.Daos;
using Pharos.Logic.BLL.DataSynchronism.Dtos;
using Pharos.Sys.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Pharos.Logic.BLL.DataSynchronism.Services
{
    public class SysStoreUserInfoDataSyncService : BaseDataSyncService<SysStoreUserInfo, SysStoreUserInfoForLocal>
    {
        public override IEnumerable<SysStoreUserInfo> Download(string storeId, string entityType)
        {
            return CurrentRepository.FindList(o => true).ToList();
        }
        public override IEnumerable<dynamic> Export(string storeId, string entityType)
        {
            return CurrentRepository.FindList(o => o.Status == 1);
        }
        public override bool Update(string key, string json, string storeId)
        {
            var datas = JsonConvert.DeserializeObject<IEnumerable<SysStoreUserInfoUpdateDao>>(json);
            foreach (var item in datas)
            {
                var repository = CurrentRepository;
                var entity = repository.Find(o => o.UserCode == item.UserCode);
                entity.LoginDT = item.LoginDT;
                repository.Update(entity);
            }
            return true;
        }
    }


}
