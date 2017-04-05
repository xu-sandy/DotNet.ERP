using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Models
{
    [NotMapped]
    public class SysMenuLimitModel:Entity.SysMenus
    {
        public string LimitIdStr { get; set; }
    }
}
