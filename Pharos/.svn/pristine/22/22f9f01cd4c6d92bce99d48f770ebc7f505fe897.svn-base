using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 优惠券卡信息
    /// </summary>
    public class CardInfo : SyncEntity
    {
        /// <summary>
        /// 卡片类型 GUID 
        /// </summary>
        public string CardTypeId { get; set; }
        /// <summary>
        /// 卡类型（1：充值卡、2:折扣卡） 
        /// </summary>
        public string CardName { get; set; }
        /// <summary>
        /// 卡类型（1：储值卡，2：购物卡；3：会员卡）
        /// </summary>
        public string CardType { get; set; }
        /// <summary>
        /// 种类(1：电子卡；2：磁卡； 3：IC 卡)
        /// </summary>
        public short Category { get; set; }
        ///// <summary>
        ///// 优惠方式(0:无，1:方案折扣，2:固定折扣)
        ///// </summary>
        //public string CouponType { get; set; }
        ///// <summary>
        ///// 折扣（优惠方式为2） 
        ///// </summary>
        //public decimal Discount { get; set; }
        ///// <summary>
        ///// 积分方式(0:无，1:方案积分）
        ///// </summary>
        //public string IntegrationType { get; set; }
        /// <summary>
        /// 最低充值金额 
        /// </summary>
        public decimal MinRecharge { get; set; }
        /// <summary>
        /// 初始金额 
        /// </summary>
        public decimal DefaultPrice { get; set; }
        /// <summary>
        /// 初始积分
        /// </summary>
        //public int DefaultIntegr { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// 状态(0:可用, 1:注销) 
        /// </summary>
        public short State { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDT { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUID { get; set; }
    }
}
