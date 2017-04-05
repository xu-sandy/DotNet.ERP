
namespace Pharos.POS.Retailing.Models.ApiReturnResults
{
    public class BillDetails
    {
        public int Index { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }
        /// <summary>
        /// 品名
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Number { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal SysPrice { get; set; }
        /// <summary>
        /// 实际售价
        /// </summary>
        public decimal ActualPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand { get; set; }
        /// <summary>
        /// 小计
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// 是否赠品
        /// </summary>
        public bool IsGift { get; set; }

        /// <summary>
        /// 商品明细销售状态（47：48：49）
        /// </summary>
        public int SalesClassifyId { get; set; }

        public string SalesClassifyTitle
        {
            get
            {
                switch (SalesClassifyId)
                {
                    case 48:
                        return "促销";
                    case 49:
                        return "赠送";
                    case 47:
                        if (PreferentialTotal > 0)
                            return "优惠";
                        else
                            return "";
                    default:
                        return "";

                }
            }
        }
        /// <summary>
        /// 优惠小计
        /// </summary>
        public decimal PreferentialTotal
        {
            get
            {

                var preferential = (SysPrice - ActualPrice) * Number
;
                return preferential > 0 ? preferential : 0;
            }
        }

    }
}
