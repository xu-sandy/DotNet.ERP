﻿using System.Collections.Generic;

namespace Pharos.Api.Retailing.Models.Pos
{
    public class PayRequest : BaseApiParams
    {
        /// <summary>
        /// 支付入口
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

        public IEnumerable<PayWay> Payway { get; set; }

        public int Reason { get; set; }
        /// <summary>
        /// 退单流水号
        /// </summary>
        public string OldOrderSn { get; set; }
      //  public ChangeRequest ChangeRecord { get; set; }
        /// <summary>
        /// 退换货、退单 旧导购员
        /// </summary>
        public string SaleMan { get; set; }


    }
}