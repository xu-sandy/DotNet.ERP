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
	/// 初始化角色子项
	/// </summary>
	[Serializable]
	public partial class ProductRole
	{
		/// <summary>
		/// 
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id { get; set; }
        public string RoleVerId { get; set; }
		/// <summary>
		/// 
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int? RoleId { get; set; }

		/// <summary>
		/// 角色名称
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string Title { get; set; }

        /// <summary>
        /// 拥有功能权限
        /// </summary>
        public string Limitids { get; set; }

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
