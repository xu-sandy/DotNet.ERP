using Pharos.Logic.BLL.DataSynchronism.Dtos;
using Pharos.Logic.Entity;
using System.Linq;

namespace Pharos.Logic.BLL.DataSynchronism.Services
{
    public class MembersDataSyncService : BaseDataSyncService<Members, MembersForLocal>
    {
        public override bool UpLoad(System.Collections.Generic.IEnumerable<Members> datas, string storeId)
        {
            var serverRepository = CurrentRepository;

            if (datas == null)
            {
                return false;
            }
            //删除重复数据
            var memberIds = CurrentRepository.Entities.Select(o => o.MemberId).ToList();
            datas = datas.Where(o => !memberIds.Exists(p => p == o.MemberId));

            var tempDatas = datas.Select(o => Pharos.Logic.Entity.BaseEntityExtension.InitEntity<Members>(o));
            serverRepository.AddRange(tempDatas.ToList());
            return true;
        }
    }
}
