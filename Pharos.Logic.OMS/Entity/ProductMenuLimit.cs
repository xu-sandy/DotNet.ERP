// --------------------------------------------------
// Copyright (C) 2017 版权所有
// 创 建 人：
// 创建时间：2017-02-14
// 描述信息：
// --------------------------------------------------

using System;

namespace Pharos.Logic.OMS.Entity
{
	/// <summary>
	/// 初始化功能模块子项
	/// </summary>
	[Serializable]
	public class ProductMenuLimit
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
		/// [长度：10]
		/// </summary>
		public int ProductId { get; set; }

		/// <summary>
		/// 
		/// [长度：10]
		/// </summary>
		public string ModuleId { get; set; }

		/// <summary>
		/// 
		/// [长度：10]
		/// </summary>
		public int MenuId { get; set; }

		/// <summary>
		/// 
		/// [长度：10]
		/// </summary>
		public int PMenuId { get; set; }

		/// <summary>
		/// 
		/// [长度：5]
		/// </summary>
		public short Depth { get; set; }

		/// <summary>
		/// 
		/// [长度：10]
		/// </summary>
		public int SortOrder { get; set; }

		/// <summary>
		/// 
		/// [长度：50]
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 
		/// [长度：200]
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// 
		/// [长度：100]
		/// </summary>
		public string Memo { get; set; }

		/// <summary>
		/// 类别(1-用户菜单，2-内部菜单，3-功能权限)
		/// [长度：5]
		/// </summary>
		public short Type { get; set; }

		/// <summary>
		/// 是否显示
		/// [长度：1]
		/// [默认值：((1))]
		/// </summary>
		public bool Status { get; set; }

		/// <summary>
		/// 是否展开
		/// [长度：1]
		/// [默认值：((1))]
		/// </summary>
		public bool Expand { get; set; }

		/// <summary>
		/// 
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
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
