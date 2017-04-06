using Pharos.Logic.ApiData.Pos.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.ValueObject
{
    public class GiftResult
    {
        public string PromotionActivity { get; set; }

        public int GiftNumber { get; set; }

        public int ToGiftNumber { get; set; }

        public decimal Price { get; set; }

        public List<GiftListItem> GiftList { get; set; }

        public string GiftId { get; set; }

        public string GiftPromotionId { get; set; }

        public GiftMode Mode { get; set; }


        public int Amount { get; set; }
    }
}
