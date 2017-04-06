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
	/// 用于管理本系统的所有日志信息
	/// </summary>
	[Serializable]
	public class SysLog
	{
		/// <summary>
		/// 记录ID
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 类型（ 1: 登录、 2: 退出、 3:异常 、4: 新增、5: 修改 、6: 删除 、10:其他 ）
		/// [长度：3]
		/// </summary>
		public byte Type { get; set; }

		/// <summary>
		/// 用户ID
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string UId { get; set; }

		/// <summary>
		/// 描述
		/// [长度：-1]
		/// [不允许为空]
		/// </summary>
		public string Summary { get; set; }

		/// <summary>
		/// 客户端IP
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string ClientIP { get; set; }

		/// <summary>
		/// 服务器名称
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string ServerName { get; set; }

		/// <summary>
		/// 记录时间
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
		/// </summary>
		public DateTime CreateDT { get; set; }

		/// <summary>
		/// 模块名称
		/// [长度：50]
		/// </summary>
		public string ModuleName { get; set; }
	}
}
