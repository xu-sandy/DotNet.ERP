using Pharos.Logic.BLL.DataSynchronism.Dtos;
using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.DataSynchronism.Services
{
    public class NoticeDataSyncService : BaseDataSyncService<Notice, NoticeForLocal>
    {
        public override IEnumerable<Notice> Download(string storeId, string entityType)
        {
            DateTime dt = DateTime.Now;
            var serverRepository = CurrentRepository;
            var sources = serverRepository.FindList(o => ("," + o.StoreId + ",").Contains("," + storeId + ",") && o.State == 1 && o.ExpirationDate > dt).ToList();
            return sources;
        }
    }
}
