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
	/// 用于管理本系统的所有行业信息
	/// </summary>
	[Serializable]
	public partial class Business
	{
		/// <summary>
		/// 
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string ById { get; set; }
        public string ParentId { get; set; }
		/// <summary>
		/// 名称
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 别名
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string Byname { get; set; }

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
		/// </summary>
		public string CreateUID { get; set; }
	}
}
