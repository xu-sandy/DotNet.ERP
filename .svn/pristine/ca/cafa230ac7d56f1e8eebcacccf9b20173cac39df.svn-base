using Pharos.Logic.BLL.DataSynchronism;
using Pharos.Logic.BLL.DataSynchronism.Dtos;
using Pharos.Sys.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.DataSynchronism.Services
{
    public class SysDataDictionaryDataSyncService : BaseDataSyncService<SysDataDictionary, SysDataDictionaryForLocal>
    {
        public override IEnumerable<SysDataDictionary> Download(string storeId, string entityType)
        {
            return base.Download(storeId, entityType).Where(o => o.Status);
        }
    }
}
