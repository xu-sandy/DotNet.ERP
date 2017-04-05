using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.LocalServices.DataSync
{
    public interface ICanUpdateEntity
    {
        bool HasUpdate { get; set; }
    }
}
