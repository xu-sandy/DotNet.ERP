// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-04-27
// 描述信息：用于管理本系统的部门或用户自定义菜单配置信息
// --------------------------------------------------

using System;

namespace Pharos.Logic.Entity
{
	/// <summary>
	/// 自定义用户菜单
	/// </summary>
	[Serializable]
	public class SysCustomMenus
	{
		/// <summary>
		/// 配置ID
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 适用类型（-1:全部,1:部门,2:角色,3:用户）
		/// [长度：10]
		/// [不允许为空]
		/// [默认值：((-1))]
		/// </summary>
		public int Type { get; set; }

		/// <summary>
		/// 适用对象ID（-1:全部,部门ID、角色ID、用户ID）
		/// [长度：10]
		/// [不允许为空]
		/// [默认值：((-1))]
		/// </summary>
		public int ObjId { get; set; }

		/// <summary>
		/// 拥有菜单项（多个间用,号间隔，-1:继承所在部门）（来自SysMenus表Id）
		/// [长度：-1]
		/// [不允许为空]
		/// [默认值：((-1))]
		/// </summary>
        public string MenuIds { get; set; }

		/// <summary>
		/// 功能Code
		/// [长度：-1]
		/// </summary>
		public string LimitsCode { get; set; }

		/// <summary>
		/// 状态（0:隐藏、1:显示）
		/// [长度：1]
		/// [默认值：((1))]
		/// </summary>
		public bool Status { get; set; }
	}
}
