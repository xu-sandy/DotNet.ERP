// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的所有相关的附件基本信息
// --------------------------------------------------

using System;

namespace Pharos.Logic.Entity
{
	/// <summary>
	/// 附件信息
	/// </summary>
	[Serializable]
	public class Attachment
	{
		/// <summary>
		/// 附件 ID
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 附件来源（ 1:合同附件、 2:财务单据、3:邮件）
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int SourceClassify { get; set; }

		/// <summary>
		/// 项目 ID
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string ItemId { get; set; }

		/// <summary>
		/// 附件名称
		/// [长度：100]
		/// [不允许为空]
		/// </summary>
		public string Title { get; set; }
		/// <summary>
		/// URL
		/// [长度：200]
		/// [不允许为空]
		/// </summary>
		public string SaveUrl { get; set; }

		/// <summary>
		/// 附件大小（ KB）
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public decimal Size { get; set; }
	}
}
