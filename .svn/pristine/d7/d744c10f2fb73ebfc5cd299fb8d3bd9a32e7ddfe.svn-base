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
	/// 管理本系统的权限控制信息
	/// </summary>
	[Serializable]
	public class SysMenuLimit
	{
		/// <summary>
		/// 
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 适用类型（0:全部,1:部门,2:角色,3:用户）
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short Type { get; set; }

		/// <summary>
		/// 适用对象ID（-1:全部,部门ID、角色ID、用户ID）
		/// [长度：2000]
		/// [不允许为空]
		/// </summary>
		public string ObjId { get; set; }

		/// <summary>
		/// 拥有菜单项
		/// [长度：4000]
		/// [不允许为空]
		/// </summary>
		public string MenuIds { get; set; }

		/// <summary>
		/// 拥有功能项
		/// [长度：4000]
		/// </summary>
		public string LimitIds { get; set; }

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
