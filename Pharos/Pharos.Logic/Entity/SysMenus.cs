// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-04-27
// 描述信息：用于管理本系统的菜单基本信息
// --------------------------------------------------

using System;

namespace Pharos.Logic.Entity
{
	/// <summary>
	/// 菜单信息
	/// </summary>
	[Serializable]
	public class SysMenus
	{
		/// <summary>
		/// 菜单ID
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 上级菜单（0:顶级）
		/// [长度：10]
		/// [不允许为空]
		/// [默认值：((-1))]
		/// </summary>
		public int PId { get; set; }

		/// <summary>
		/// 排序（0:无）
		/// [长度：10]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public int SortOrder { get; set; }

		/// <summary>
		/// 名称
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 
		/// [长度：200]
		/// [不允许为空]
		/// </summary>
		public string URL { get; set; }

		/// <summary>
		/// 功能Code
		/// [长度：1000]
		/// </summary>
		public string LimitsCode { get; set; }

		/// <summary>
		/// 状态（0:隐藏、1:显示）
		/// [长度：1]
		/// [不允许为空]
		/// [默认值：((1))]
		/// </summary>
		public bool Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="limit"></param>
        public void Replace(SysMenus model)
        {
            Id = model.Id;
            PId = model.PId;
            Title = model.Title;
            Status = model.Status;
            LimitsCode = model.LimitsCode;
            SortOrder = model.SortOrder;
            URL = model.URL;
        }
	}
}
