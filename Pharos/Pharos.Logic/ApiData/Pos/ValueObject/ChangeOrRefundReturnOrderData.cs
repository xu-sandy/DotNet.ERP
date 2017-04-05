using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.ValueObject
{
    public class ChangeOrRefundReturnOrderData
    {
        public SaleManInfo SaleMan { get; set; }

        public List<ChangingList> OldOrderList { get; set; }
    }
}
