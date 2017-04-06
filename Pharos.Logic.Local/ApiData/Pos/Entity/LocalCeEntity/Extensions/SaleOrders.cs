using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity
{
    public partial class SaleOrders
    {
        public virtual List<ConsumptionPayment> ConsumptionPayments { get; set; }
        public virtual List<SaleDetail> SaleDetails { get; set; }
        public virtual List<WipeZero> WipeZeros { get; set; }

    }
}
