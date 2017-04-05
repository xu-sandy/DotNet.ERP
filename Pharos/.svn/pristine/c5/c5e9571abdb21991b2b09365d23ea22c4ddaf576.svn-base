// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的所有相关的附件基本信息
// --------------------------------------------------

using System;

namespace Pharos.Logic.OMS.Entity
{
	/// <summary>
	/// 附件信息
	/// </summary>
	[Serializable]
	public partial class Attachment
	{
		/// <summary>
		/// 附件 ID
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 项目 ID
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string ItemId { get; set; }

		/// <summary>
        /// 附件原名称
		/// [长度：100]
		/// [不允许为空]
		/// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 附件新名称
        /// </summary>
        public string NewName { get; set; }
		/// <summary>
		/// URL
		/// [长度：200]
		/// [不允许为空]
		/// </summary>
		public string Path { get; set; }
        /// <summary>
        /// 使用关联表
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string ExtName { get; set; }
		/// <summary>
		/// 附件大小（ KB）
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
        public int FileSize { get; set; }
        public string CreateUID { get; set; }
        public DateTime CreateDT { get; set; }
	}
}
