using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SyncService.SyncEntities
{
    [Serializable]
    public class ConsumptionPayment : SyncDataObject
    {
        public int CompanyId { get; set; }

        /// <summary>
        /// 流水号
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        public string PaySN { get; set; }


        /// <summary>
        /// 支付方式 ID
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int ApiCode { get; set; }


        /// <summary>
        /// 交易单号（由支付宝或微信返回）
        /// [长度：50]
        /// </summary>
        public string ApiOrderSN { get; set; }


        /// <summary>
        /// 金额
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// </summary>
        public decimal Amount { get; set; }


        /// <summary>
        /// 备注
        /// [长度：200]
        /// </summary>
        public string Memo { get; set; }


        /// <summary>
        /// 支付状态（0：未支付、1：已支付）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public short State { get; set; }

        public string CardNo { get; set; }

        public decimal Change { get; set; }

        public decimal Received { get; set; }
    }
}
