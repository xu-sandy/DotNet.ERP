// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：黄森
// 创建时间：2015-11-10
// 描述信息：用于管理本系统的所有货品报损的明细信息
// --------------------------------------------------

using System;

namespace Pharos.Logic.Entity
{
	/// <summary>
	/// 报损明细信息
	/// </summary>
	[Serializable]
    public partial class BreakageList
	{
		/// <summary>
		/// 明细ID
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
        public int Id { get; set; }

		/// <summary>
		/// 报损单ID
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
        public string BreakageGoodsId { get; set; }

		/// <summary>
		/// 条码
		/// [长度：30]
		/// [不允许为空]
		/// </summary>
		public string Barcode { get; set; }

		/// <summary>
		/// 报损数量
		/// [不允许为空]
		/// </summary>
        public decimal BreakageNumber { get; set; }

		/// <summary>
		/// 报损价（出库：批发价,入库：进价）
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
        public decimal BreakagePrice { get; set; }

		/// <summary>
        /// 状态（ 0:未审核、 1:已审核）
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public short State { get; set; }
        

	}
}
