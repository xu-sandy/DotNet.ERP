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
	/// 用于管理本系统的所有优惠券适用范围信息 
	/// </summary>
	[Serializable]
	public class CouponApplyDetails
	{
		/// <summary>
		/// 记录 ID 
		/// [主键：√]
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
		/// 优惠券制作 id 
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int MakingCouponCardId { get; set; }

		/// <summary>
		/// 适用商品类型 
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short ProductType { get; set; }

		/// <summary>
		/// 
		/// [长度：-1]
		/// </summary>
		public string Product { get; set; }
	}
}
