using Pharos.Logic.BLL.DataSynchronism.Dtos;
using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.DataSynchronism.Services
{
    public class SalesReturnsDetailedDataSyncService : BaseDataSyncService<SalesReturnsDetailed, SalesReturnsDetailedForLocal>
    {
        public override IEnumerable<SalesReturnsDetailed> Download(string storeId, string entityType)
        {
            SalesReturnsDetailedDataSyncService.IsForcedExpired = true;

            var expirationDate = DateTime.Now.Date.AddYears(-1);

            var query = (from a in CurrentRepository.Entities
                         from b in SalesReturnsDataSyncService.CurrentRepository.Entities
                         where a.ReturnId == b.ReturnId && b.StoreId == storeId && b.CreateDT > expirationDate
                         select a).ToList();

            return query;
        }
        public override bool UpLoad(IEnumerable<SalesReturnsDetailed> datas, string storeId)
        {
            var serverRepository = CurrentRepository;

            if (datas == null)
            {
                return false;
            }
            //删除重复数据
            var returnIds = CurrentRepository.Entities.Select(o => o.ReturnId).ToList();
            datas = datas.Where(o => !returnIds.Exists(p => p == o.ReturnId));

            var tempDatas = datas.Select(o => Pharos.Logic.Entity.BaseEntityExtension.InitEntity<SalesReturnsDetailed>(o));
            serverRepository.AddRange(tempDatas.ToList());
            return true;
        }
    }
}
