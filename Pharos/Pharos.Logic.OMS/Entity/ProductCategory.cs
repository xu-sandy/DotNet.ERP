// --------------------------------------------------
// Copyright (C) 2016 版权所有
// 创 建 人：蔡少发
// 创建时间：2016-09-03
// 描述信息：
// --------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharos.Logic.OMS.Entity
{
	/// <summary>
	/// 用于管理本系统的所有相关的附件基本信息
	/// </summary>
	[Serializable]
	public partial class ProductCategory
	{
		/// <summary>
		/// 记录ID
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 信息来源（1-本司2-商户）
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short Source { get; set; }

		/// <summary>
		/// 分类编号（全局唯一）
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int CategorySN { get; set; }

		/// <summary>
		/// 上级分类SN
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int CategoryPSN { get; set; }

		/// <summary>
		/// 分类层级（1:顶级、2：二级、3:三级、4:四级）
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((1))]
		/// </summary>
		public short Grade { get; set; }

		/// <summary>
		/// 分类代码 （最大 99，同一级分类下唯一）
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short CategoryCode { get; set; }

		/// <summary>
		/// 分类名称
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 顺序
		/// [长度：10]
		/// </summary>
		public int OrderNum { get; set; }

		/// <summary>
		/// 状态（0:禁用、1:可用）
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
        public short State { get { return state; } set { state = value; } }

        short state = 1;
	}
}
