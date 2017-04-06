using Pharos.Logic.BLL.DataSynchronism;
using Pharos.Logic.BLL.DataSynchronism.Dtos;
using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.DataSynchronism.Services
{
    public class SalesReturnsDataSyncService : BaseDataSyncService<SalesReturns, SalesReturnsForLocal>
    {
        public override IEnumerable<SalesReturns> Download(string storeId, string entityType)
        {
            SalesReturnsDataSyncService.IsForcedExpired = true;

            var expirationDate = DateTime.Now.Date.AddYears(-1);

            var query = (from a in CurrentRepository.Entities
                         where a.StoreId == storeId && a.CreateDT > expirationDate
                         select a).ToList();
            return query;
        }
        public override bool UpLoad(IEnumerable<SalesReturns> datas, string storeId)
        {
            var serverRepository = CurrentRepository;

            if (datas == null)
            {
                return false;
            }
            //删除重复数据
            var ReturnIds = CurrentRepository.Entities.Select(o => o.ReturnId).ToList();
            datas = datas.Where(o => !ReturnIds.Exists(p => p == o.ReturnId));

            var tempDatas = datas.Select(o => Pharos.Logic.Entity.BaseEntityExtension.InitEntity<SalesReturns>(o));
            serverRepository.AddRange(tempDatas.ToList());
            return true;
        }
    }
}
