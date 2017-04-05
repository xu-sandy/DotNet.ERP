// --------------------------------------------------
// Copyright (C) 2016 版权所有
// 创 建 人：蔡少发
// 创建时间：2016-08-02
// 描述信息：
// --------------------------------------------------

using System;

namespace Pharos.Logic.Entity
{
	/// <summary>
	/// 用于管理本系统的所有优惠券信息 
	/// </summary>
	[Serializable]
	public class Coupons
	{
		/// <summary>
		/// 记录 ID 
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 公司CID
		/// [长度：10]
		/// [不允许为空]
		/// [默认值：((-1))]
		/// </summary>
		public int CompanyId { get; set; }

		/// <summary>
		/// 制卡批次 
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string BatchSN { get; set; }

		/// <summary>
		/// 卡号 
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string CouponSN { get; set; }

		/// <summary>
		/// 券类别 
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short CouponType { get; set; }

		/// <summary>
		/// 优惠券来源（从哪个门店领取， -1 为其他） 
		/// [长度：10]
		/// [默认值：((-1))]
		/// </summary>
		public int StoreId { get; set; }
        /// <summary>
        /// 领用人,如会员
        /// </summary>
        public string UsedUser { get; set; }
		/// <summary>
		/// 领用时间 
		/// [长度：23，小数位数：3]
		/// [默认值：(getdate())]
		/// </summary>
		public DateTime UsedDate { get; set; }

		/// <summary>
		/// 状态（1：未使用；2：已使用；3 已作废）
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short State { get; set; }

		/// <summary>
		/// 
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public string ExpiryStart { get; set; }

		/// <summary>
		/// 有效期-截止(空为永久)
		/// [长度：10]
		/// </summary>
		public string ExpiryEnd { get; set; }

		/// <summary>
		/// 派发员 
		/// [长度：40]
		/// </summary>
		public string Distribute { get; set; }
	}
}
