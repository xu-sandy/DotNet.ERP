using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharos.CRM.Retailing.Models
{
    public class OrdersDetails
    {

        public object Statistics { get; set; }

        public IEnumerable<object> Gifts { get; set; }

        public IEnumerable<object> BuyList { get; set; }
 
    }
}