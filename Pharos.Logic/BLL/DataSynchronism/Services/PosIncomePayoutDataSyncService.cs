using Pharos.Logic.BLL.DataSynchronism.Dtos;
using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pharos.Logic.BLL.DataSynchronism.Services
{
    public class PosIncomePayoutDataSyncService : BaseDataSyncService<PosIncomePayout, PosIncomePayoutForLocal>
    {
        public override System.Collections.Generic.IEnumerable<PosIncomePayout> Download(string storeId, string entityType)
        {
            var expirationDate = DateTime.Now.Date.AddYears(-1);

            var query = (from a in CurrentRepository.Entities
                         where a.CreateDT > expirationDate && a.StoreId == storeId
                         select a).ToList();

            return query;
        }
        public override bool UpLoad(IEnumerable<PosIncomePayout> datas, string storeId)
        {
            var serverRepository = CurrentRepository;

            if (datas == null)
            {
                return false;
            }
            //删除重复数据
            if (datas.Count() == 0)
            {
                return true;
            }
            var info = datas.FirstOrDefault();
            var date = datas.Min(o => o.CreateDT);
            var deleteDatas = CurrentRepository.Entities.Where(o => o.StoreId == storeId && o.MachineSN == info.MachineSN && o.CreateDT >= date);
            var tempNewDatas = new List<PosIncomePayout>();
            foreach (var item in datas)
            {
                var result = true;
                foreach (var obj in deleteDatas)
                {
                    if (item.CreateUID == obj.CreateUID && (item.CreateDT - obj.CreateDT).Duration() < new TimeSpan(0, 0, 1))
                    {
                        result = false;
                    }
                }
                if (result)
                    tempNewDatas.Add(item);

            }


            var tempDatas = tempNewDatas.Select(o => Pharos.Logic.Entity.BaseEntityExtension.InitEntity<PosIncomePayout>(o));
            serverRepository.AddRange(tempDatas.ToList());
            return true;

        }
    }

}