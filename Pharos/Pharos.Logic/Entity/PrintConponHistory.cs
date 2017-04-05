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
	/// 用于管理本系统的优惠券历史信息 
	/// </summary>
	[Serializable]
	public class PrintConponHistory
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
		/// 制卡批次 
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string BatchSN { get; set; }

		/// <summary>
		/// 券类别 
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short CouponType { get; set; }

		/// <summary>
		/// 卡号 
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string CouponSN { get; set; }

		/// <summary>
		/// 
		/// [长度：40]
		/// </summary>
		public string SecurityCode { get; set; }

		/// <summary>
		/// 创建时间
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
		/// </summary>
		public DateTime CreateDT { get; set; }

		/// <summary>
		/// 创建人
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string CreateUID { get; set; }
	}
}
