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
	/// 历史数据处理
	/// </summary>
	[Serializable]
	public class ProductPublishSql
	{
		/// <summary>
		/// 
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 发布编号
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int PublishId { get; set; }

		/// <summary>
		/// 归属菜单
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int MenuId { get; set; }

		/// <summary>
		/// 排序
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public int RunSort { get; set; }

		/// <summary>
		/// 执行Sql
		/// [长度：-1]
		/// [不允许为空]
		/// </summary>
		public string RunSql { get; set; }

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
