using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.OMS.Entity;
namespace Pharos.Logic.OMS.IDAL
{
    public interface IDictRepository:IBaseRepository<SysDataDictionary>
    {
        List<SysDataDictionaryExt> GetPageList(int pageIndex, int pageSize, string key);
    }
}
