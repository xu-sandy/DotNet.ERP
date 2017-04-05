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
	/// 角色信息
	/// </summary>
	[Serializable]
	public partial class SysRoles
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
		public int RoleId { get; set; }

		/// <summary>
		/// 
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 类型（0-默认，1-自定义）
		/// [长度：5]
		/// </summary>
		public short Type { get; set; }

		/// <summary>
		/// 适用部门
		/// [长度：2000]
		/// </summary>
		public string DeptId { get; set; }
        /// <summary>
        /// 拥有功能权限
        /// </summary>
        public string Limitids { get; set; }
		/// <summary>
		/// 
		/// [长度：500]
		/// </summary>
		public string Memo { get; set; }

		/// <summary>
		/// 状态( 0- 关闭1-可用)
		/// [长度：1]
		/// [不允许为空]
		/// [默认值：((1))]
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
