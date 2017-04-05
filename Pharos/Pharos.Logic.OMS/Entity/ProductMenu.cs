// --------------------------------------------------
// Copyright (C) 2016 版权所有
// 创 建 人：linbl
// 创建时间：2016-12-30
// 描述信息：
// --------------------------------------------------

using System;

namespace Pharos.Logic.OMS.Entity
{
	/// <summary>
	/// 产品对应的菜单信息
	/// </summary>
	[Serializable]
	public partial class ProductMenu
	{
		/// <summary>
		/// 
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string ProductId { get; set; }

		/// <summary>
		/// 
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int MenuId { get; set; }

		/// <summary>
		/// 
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int PMenuId { get; set; }

		/// <summary>
		/// 
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short Depth { get; set; }

		/// <summary>
		/// 
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short SortOrder { get; set; }

		/// <summary>
		/// 
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 
		/// [长度：50]
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// 类别(1-用户菜单，2-内部菜单)
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((1))]
		/// </summary>
		public short Type { get; set; }

		/// <summary>
		/// 是否可用
		/// [长度：1]
		/// [不允许为空]
		/// [默认值：((1))]
		/// </summary>
		public bool Status { get; set; }

		/// <summary>
		/// 是否展开
		/// [长度：1]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public bool Expand { get; set; }

		/// <summary>
		/// getdate()
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// </summary>
		public DateTime CreateDT { get; set; }

		/// <summary>
		/// 
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string CreateUID { get; set; }
	}
}
