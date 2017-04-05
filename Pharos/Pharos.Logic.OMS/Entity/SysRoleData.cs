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
    /// 初始化角色子项(拥有菜单项和功能权限)
	/// </summary>
	[Serializable]
	public class SysRoleData
	{
		/// <summary>
		/// 
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id { get; set; }
        public int RoleId { get; set; }
		/// <summary>
		/// 
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int MenuId { get; set; }
        public int PMenuId { get; set; }
        
		/// <summary>
		/// 排序
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool HasSelected { get; set; }
	}
}
