using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.ApiPos
{
    /// <summary>
    /// 门店
    /// </summary>
    public class Store
    {
        public List<PosMachine> Machines { get; set; }

        public string StoreId { get; set; }

        public string Name { get; set; }



    }
}
