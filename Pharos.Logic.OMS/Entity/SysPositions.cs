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
	/// 职位信息
	/// </summary>
	[Serializable]
	public class SysPositions
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
		public string PositId { get; set; }

		/// <summary>
        /// 代码
		/// [长度：30]
		/// [不允许为空]
		/// </summary>
		public string Code { get; set; }

		/// <summary>
        /// 适用部门(多个以逗号隔开)
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public string DeptId { get; set; }

		/// <summary>
		/// 
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 
		/// [长度：1]
		/// [不允许为空]
		/// </summary>
		public bool Status { get; set; }

		/// <summary>
		/// 
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
		/// </summary>
		public DateTime UpdateDT { get; set; }

		/// <summary>
		/// 
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string UpdateUID { get; set; }

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
