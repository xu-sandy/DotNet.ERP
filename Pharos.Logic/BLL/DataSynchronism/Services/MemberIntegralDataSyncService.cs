using Pharos.Logic.BLL.DataSynchronism.Dtos;
using Pharos.Logic.Entity;
using System.Linq;

namespace Pharos.Logic.BLL.DataSynchronism.Services
{
    public class MemberIntegralDataSyncService : BaseDataSyncService<MemberIntegral, MemberIntegralForLocal>
    {
        public override bool UpLoad(System.Collections.Generic.IEnumerable<MemberIntegral> datas, string storeId)
        {
            var serverRepository = CurrentRepository;

            if (datas == null)
            {
                return false;
            }
            //删除重复数据
            var paySNs = CurrentRepository.Entities.Select(o => o.PaySN).ToList();
            datas = datas.Where(o => !paySNs.Exists(p => p == o.PaySN));

            var tempDatas = datas.Select(o => Pharos.Logic.Entity.BaseEntityExtension.InitEntity<MemberIntegral>(o));
            serverRepository.AddRange(tempDatas.ToList());
            return true;
        }
    }
}
