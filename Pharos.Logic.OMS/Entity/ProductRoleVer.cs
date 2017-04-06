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
	/// 初始角色版本
	/// </summary>
	[Serializable]
	public partial class ProductRoleVer
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
		/// 
		/// [长度：10]
		/// </summary>
		public string RoleVerId { get; set; }

		/// <summary>
		/// 
		/// [长度：10]
		/// </summary>
		public int ProductId { get; set; }
        /// <summary>
        /// 使用的初始功能
        /// </summary>
        public string ModuleId { get; set; }
		/// <summary>
		/// 状态（0-未生效，1-已生效，2-已失效）
		/// [长度：5]
		/// </summary>
		public short Status { get; set; }

		/// <summary>
		/// 版本状态（0-未发布，1-测试版，2-正式版）
		/// [长度：5]
		/// </summary>
		public short VerStatus { get; set; }

		/// <summary>
		/// 
		/// [长度：19，小数位数：4]
		/// </summary>
		public decimal VerCode { get; set; }

		/// <summary>
		/// 
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
		/// </summary>
		public DateTime UpdateDT { get; set; }

		/// <summary>
		/// 
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string UpdateUID { get; set; }

		/// <summary>
		/// 
		/// [长度：23，小数位数：3]
		/// </summary>
		public DateTime? PublishDT { get; set; }

		/// <summary>
		/// 
		/// [长度：40]
		/// </summary>
		public string PublishUID { get; set; }

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
