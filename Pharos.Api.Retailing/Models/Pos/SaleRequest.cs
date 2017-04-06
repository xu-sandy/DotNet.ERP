using Pharos.Logic.ApiData.Pos.Sale;

namespace Pharos.Api.Retailing.Models
{
    public class SaleRequest : BaseApiParams
    {
        public string Barcode { get; set; }

        public decimal Number { get; set; }

        public decimal SalePrice { get; set; }

        public SaleStatus Status { get; set; }

        public string GiftId { get; set; }

        public string GiftPromotionId { get; set; }


    }
}