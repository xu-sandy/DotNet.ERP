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
	/// 初始字典数据子项
	/// </summary>
	[Serializable]
	public class ProductDictionaryData
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
		/// [不允许为空]
		/// </summary>
		public string DictId { get; set; }

		/// <summary>
		/// 
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int DicSN { get; set; }

		/// <summary>
		/// 
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int DicPSN { get; set; }

		/// <summary>
		/// 
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int SortOrder { get; set; }

		/// <summary>
		/// 
		/// [长度：50]
		/// [不允许为空]
		/// [默认值：(N'类别名称')]
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short Depth { get; set; }

		/// <summary>
		/// 是否有子项
		/// [长度：1]
		/// [不允许为空]
		/// </summary>
		public bool HasChild { get; set; }

		/// <summary>
		/// 状态(0-停用1-可用)
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((1))]
		/// </summary>
		public short Status { get; set; }

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
