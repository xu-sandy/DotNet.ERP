using Pharos.Logic.DataSynchronism.ObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.DataSynchronism
{
    public interface IUpdateProvider<TDto>
    {
         DataSyncResult SaveChanges(IEnumerable<TDto> datas);
    }
}
