// --------------------------------------------------
// Copyright (C) 2016 版权所有
// 创 建 人：蔡少发
// 创建时间：2016-09-03
// 描述信息：
// --------------------------------------------------

using System;

namespace Pharos.Logic.OMS.Entity
{
	/// <summary>
	/// 用于管理本系统的产品档案所附属的品牌信息
	/// </summary>
	[Serializable]
	public class ProductBrand
	{
		/// <summary>
		/// 记录ID
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id { get; set; }

		/// <summary>
        /// 来源商户（0-本司)
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public int Source { get; set; }

		/// <summary>
		/// 品牌分类ID
		/// [长度：10]
		/// [不允许为空]
		/// [默认值：((-1))]
		/// </summary>
		public int ClassifyId { get; set; }

		/// <summary>
		/// 品牌编号（全局唯一)
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int BrandSN { get; set; }

		/// <summary>
		/// 品牌名称
		/// [长度：20]
		/// [不允许为空]
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 品牌简拼
		/// [长度：20]
		/// </summary>
		public string JianPin { get; set; }

		/// <summary>
		/// 状态（0:禁用、1:可用）
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((1))]
		/// </summary>
		public short State { get; set; }
	}
}
