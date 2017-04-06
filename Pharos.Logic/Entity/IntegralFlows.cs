using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public class IntegralFlows : BaseEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 门店
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 交易单号
        /// </summary>
        public string FlowSN { get; set; }
        /// <summary>
        /// 订单总额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 交易积分
        /// </summary>
        public decimal Integral { get; set; }
        /// <summary>
        /// 抵扣金额
        /// </summary>
        public decimal DiscountAmount { get; set; }
        /// <summary>
        /// 会员卡号
        /// </summary>
        public string CardSN { get; set; }
        /// <summary>
        /// 实收
        /// </summary>
        public decimal Received { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 赠送状态（0：未生效，1：已生效）
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string MemberId { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }
    }
}
