// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的所有相关的财务单据凭证信息
// --------------------------------------------------

using System;

namespace Pharos.Logic.Entity
{
	/// <summary>
	/// 单据信息表
	/// </summary>
	[Serializable]
	public partial class Receipts:BaseEntity
	{
		/// <summary>
		/// 单据ID
		/// [主键：√]
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// 仓库/门店ID
		/// [长度：3]
		/// </summary>
		public string StoreId { get; set; }

		/// <summary>
		/// 单据类别ID（来自数据字典表）
		/// [长度：10]
		/// [不允许为空]
		/// [默认值：((-1))]
		/// </summary>
		public int CategoryId { get; set; }

		/// <summary>
		/// 单据编号
		/// [长度：30]
		/// [不允许为空]
		/// </summary>
		public string Number { get; set; }

		/// <summary>
		/// 金额
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// </summary>
		public decimal Amount { get; set; }

		/// <summary>
		/// 张数
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short Pages { get; set; }

		/// <summary>
		/// 抬头
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 备注
		/// [长度：200]
		/// </summary>
		public string Memo { get; set; }

		/// <summary>
		/// 交单人UID
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
        [Pharos.Utility.Exclude()]
		public string CreateUID { get; set; }

		/// <summary>
		/// 交单日期
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
		/// </summary>
        [Pharos.Utility.Exclude()]
		public DateTime CreateDT { get; set; }

		/// <summary>
		/// 状态（0:未提交、1:已审核、2:已结)
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
        [Pharos.Utility.Exclude()]
		public short State { get; set; }

        /// <summary>
        /// 单据来源（1:采购方、2:供应商）
        /// [不允许为空]
        /// </summary>
        [Pharos.Utility.Exclude()]
        public short Source { get; set; }
	}
}
