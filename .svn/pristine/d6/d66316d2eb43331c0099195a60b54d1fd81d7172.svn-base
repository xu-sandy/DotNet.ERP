using Pharos.POS.Retailing.Models.PosModels;
using System.Collections.Generic;

namespace Pharos.POS.Retailing.Models.ApiParams
{
    public class ApiPayParams : BaseApiParams
    {
        /// <summary>
        /// 销售、退换货
        /// </summary>
        public PayAction Mode { get; set; }
        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal Receivable { get; set; }
        /// <summary>
        /// 订单金额（应收金额）
        /// </summary>
        public decimal OrderAmount { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public IEnumerable<PayWay> Payway { get; set; }
        /// <summary>
        /// 退换货原因信息
        /// </summary>
        public int Reason { get; set; }
        /// <summary>
        /// 退单流水号
        /// </summary>
        public string OldOrderSn { get; set; }

    }
    /// <summary>
    /// 支付方式
    /// </summary>
    public class PayWay
    {
        /// <summary>
        /// 类型
        /// </summary>
        public PayMode Type { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 实收
        /// </summary>
        public decimal Receive { get; set; }
        /// <summary>
        /// 找零
        /// </summary>
        public decimal Change { get; set; }

        /// <summary>
        /// 卡号或扫码编号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 抹零金额
        /// </summary>
        public decimal WipeZero { get; set; }
    }
    /// <summary>
    /// 退换货信息
    /// </summary>
    public class ChangeRequest
    {
        //  public List<ChangeRefundItem> ChangeList { get; set; }
        /// <summary>
        /// 退换货原因
        /// </summary>
        public int Reason { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public string PaySn { get; set; }
    }
}
