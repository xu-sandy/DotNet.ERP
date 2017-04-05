// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-04-27
// 描述信息：用于管理本系统的权限组信息
// --------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharos.Logic.Entity
{
	/// <summary>
	/// 权限分组
	/// </summary>
	[Serializable]
	public class SysRoles
	{
		/// <summary>
		/// 权限组ID
		/// [主键：√]
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public int Id { get; set; }
        /// <summary>
        /// 角色编号
        /// </summary>
        public int RoleId { get; set; }
		/// <summary>
		/// 组名称
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 功能Code（多个间用,号间隔，-1:不限）
		/// [长度：4000]
		/// [不允许为空]
		/// [默认值：((-1))]
		/// </summary>
		public string LimitsIds { get; set; }

		/// <summary>
		/// 描述说明
		/// [长度：100]
		/// </summary>
		public string Memo { get; set; }

		/// <summary>
		/// 状态（0:关闭、1:可用）
		/// [长度：1]
		/// [不允许为空]
		/// [默认值：((1))]
		/// </summary>
		public bool Status { get; set; }
	}
}
