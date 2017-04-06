using Pharos.Logic.ApiData.Pos.ValueObject;
using System.Collections.Generic;

namespace Pharos.Api.Retailing.Models
{
    public class OrdersDetails
    {

        public SaleStatistics Statistics { get; set; }

        public IEnumerable<GiftResult> Gifts { get; set; }

        public IEnumerable<OrderItem> BuyList { get; set; }
 
    }
}