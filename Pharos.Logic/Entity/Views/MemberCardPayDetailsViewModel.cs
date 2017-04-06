using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity.Views
{
    public class MemberCardPayDetailsViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderSN { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardSN { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 卡类型
        /// </summary>
        public string CardType { get; set; }
        /// <summary>
        /// 门店
        /// </summary>
        public string StoreTitle { get; set; }
        /// <summary>
        /// 订单时间
        /// </summary>
        public DateTime CreateDT { get; set; }
    }
}
