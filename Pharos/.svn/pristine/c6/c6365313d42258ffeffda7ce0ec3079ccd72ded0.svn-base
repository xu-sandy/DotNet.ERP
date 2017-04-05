using Pharos.Logic.BLL.DataSynchronism.Dtos;
using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.DataSynchronism.Services
{
    public class ApiLibraryDataSyncService : BaseDataSyncService<ApiLibrary, ApiLibraryForLocal>
    {
        public override IEnumerable<dynamic> Export(string storeId, string entityType)
        {
            var serverRepository = CurrentRepository;
            var type = new int[] { 43, 98, 101 };
            var sources = serverRepository.FindList(o =>type.Contains(o.ApiType)).ToList();
            return sources;
        }
    }
}
