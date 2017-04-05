// --------------------------------------------------
// Copyright (C) 2017 版权所有
// 创 建 人：
// 创建时间：2017-02-14
// 描述信息：
// --------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharos.Logic.OMS.Entity
{
	/// <summary>
	/// 产品版本
	/// </summary>
	[Serializable]
	public class ProductVer
	{
		/// <summary>
		/// 
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		/// <summary>
		/// 产品编号
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int? ProductId { get; set; }

		/// <summary>
		/// 产品名称
		/// [长度：50]
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 产品别名
		/// [长度：50]
		/// </summary>
		public string Alias { get; set; }

		/// <summary>
		/// 系统名称
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string SysName { get; set; }

		/// <summary>
        /// 状态（0-未生效，1-已生效，2-已失效）
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short Status { get; set; }

		/// <summary>
		/// 描述
		/// [长度：500]
		/// </summary>
		public string Memo { get; set; }

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
