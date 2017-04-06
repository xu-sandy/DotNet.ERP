// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-04-27
// 描述信息：用于管理本系统的用户自定义权限信息
// --------------------------------------------------

using System;

namespace Pharos.Logic.Entity
{
	/// <summary>
	/// 自定义用户权限
	/// </summary>
	[Serializable]
	public class SysUsersLimits
	{
		/// <summary>
		/// 记录ID
		/// [主键：√]
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// 用户ID
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string UId { get; set; }

		/// <summary>
		/// 功能Code
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int LimitsCode { get; set; }
	}
}
