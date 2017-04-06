using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 销售订单(修改)历史
    /// 目前只记录Server端的修改订单操作历史
    /// </summary>
    public class SaleOrderHistory
    {
        /// <summary>
        /// 记录ID
        /// [主键：√]
        /// [自增]
        /// [不允许为空]
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 退货Id
        /// [不允许为空]
        /// </summary>
        public string ReturnId { get; set; }
        /// <summary>
        /// 流水号
        /// [不允许为空]
        /// </summary>
        public string PaySN { get; set; }
        /// <summary>
        /// 原金额合计（优惠后）
        /// [不允许为空]
        /// </summary>
        public decimal PreviousTotalAmount { get; set; }
        /// <summary>
        /// 原优惠合计
        /// [不允许为空]
        /// </summary>
        public decimal PreviousPreferentialPrice { get; set; }
    }
}
