using Pharos.Logic.BLL.DataSynchronism.Dtos;
using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.DataSynchronism.Services
{
    public class DeviceRegInfoDataSyncService : BaseDataSyncService<DeviceRegInfo, DeviceRegInfoForLocal>
    {
        public override IEnumerable<DeviceRegInfo> Download(string storeId, string entityType)
        {
            return CurrentRepository.Entities.Where(o => o.StoreId == storeId && o.State == 1);
        }
    }
}
