using Pharos.POS.Retailing.Models.PosModels;
using System.Collections.Generic;

namespace Pharos.POS.Retailing.Models.ApiParams
{
    /// <summary>
    /// 换货无需支付提交参数
    /// </summary>
    public class NoNeedPaySaveParams : BaseApiParams
    {
        /// <summary>
        /// 退换货商品列表
        /// </summary>
        public IEnumerable<ChangeRefundItem> ChangeRefundItem { get; set; }
        /// <summary>
        /// 退换货原因
        /// </summary>
        public int Reason { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public string PaySn { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 退换货状态
        /// </summary>
        public ChangeStatus Mode { get; set; }
    }
    /// <summary>
    /// 退换货明细
    /// </summary>
    public class ChangeRefundItem
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Number { get; set; }

    }
}
