// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的商品满元促销活动信息（主表：Bundling）
// --------------------------------------------------

using System;

namespace Pharos.Logic.Entity
{
	/// <summary>
	/// 满元促销
	/// </summary>
	[Serializable]
	public class QuotaPromotion
	{
		/// <summary>
		/// 
		/// [主键：√]
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// 消费额度
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public decimal OrderAmount { get; set; }

		/// <summary>
		/// 促销形式（1:现金返回、2:代金券、3:折扣、4:加购按具体商品赠送、5:加购按单价范围赠送）
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short PromotionType { get; set; }

		/// <summary>
		/// 金额或折扣率
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// </summary>
		public decimal Discount { get; set; }

		/// <summary>
		/// 组合单品条码（多个以,号间隔）
		/// [长度：4000]
		/// </summary>
		public string Barcode { get; set; }

		/// <summary>
		/// 组合商品系列ID（多个以,号间隔）
		/// [长度：4000]
		/// </summary>
		public string CategorySN { get; set; }

		/// <summary>
		/// 不参与单品条码（多个以,号间隔）
		/// [长度：4000]
		/// </summary>
		public string ExclBarcode { get; set; }

		/// <summary>
		/// 不参与商品系列ID（多个以,号间隔）
		/// [长度：4000]
		/// </summary>
		public string ExclCategorySN { get; set; }

		/// <summary>
		/// 允许累加赠送（0:不允许、1:允许）
		/// [长度：5]
		/// [默认值：((1))]
		/// </summary>
		public short AllowedAccumulate { get; set; }
	}
}
