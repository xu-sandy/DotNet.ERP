// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-07-24
// 描述信息：用于管理本系统的所有库存货品盘点信息（依赖表：TreasuryLocks）
// --------------------------------------------------

using System;

namespace Pharos.Logic.Entity
{
	/// <summary>
	/// 库存盘点
	/// </summary>
	[Serializable]
    public partial class StockTaking:BaseEntity
	{
		/// <summary>
		/// 记录ID
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 盘点批次
		/// [长度：30]
		/// [不允许为空]
		/// </summary>
		public string CheckBatch { get; set; }

		/// <summary>
		/// 商品条码
		/// [长度：30]
		/// [不允许为空]
		/// </summary>
		public string Barcode { get; set; }

		/// <summary>
        /// 当前库存量
		/// [长度：10]
		/// </summary>
		public decimal LockNumber { get; set; }

		/// <summary>
		/// 实盘数量
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public decimal? ActualNumber { get; set; }
        /// <summary>
        /// 0-未确，1-无差异，2有差异
        /// </summary>
        public short Sure { get; set; }
		/// <summary>
		/// 创建时间
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
		/// </summary>
		public DateTime CreateDT { get; set; }

		/// <summary>
		/// 创建人UID
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string CreateUID { get; set; }
	}
}
