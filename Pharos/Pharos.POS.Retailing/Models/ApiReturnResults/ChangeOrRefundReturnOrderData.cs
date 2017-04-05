using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.POS.Retailing.Models.ApiReturnResults
{
    public class ChangeOrRefundReturnOrderData
    {
        public SaleManInfo SaleMan { get; set; }

        public IEnumerable<ChangingList> OldOrderList { get; set; }
    }
}
