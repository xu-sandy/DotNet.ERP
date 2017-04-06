// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-07-24
// 描述信息：用于管理本系统的所有货品出库明细清单信息
// --------------------------------------------------

using System;

namespace Pharos.Logic.Entity
{
	/// <summary>
	/// 出库明细信息
	/// </summary>
	[Serializable]
    public partial class OutboundList
	{
		/// <summary>
		/// 明细 ID
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 出库单ID
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string OutboundId { get; set; }

		/// <summary>
		/// 商品条码
		/// [长度：30]
		/// [不允许为空]
		/// </summary>
		public string Barcode { get; set; }

		/// <summary>
		/// 出库数量
		/// [不允许为空]
		/// [默认值：((1))]
		/// </summary>
		public decimal OutboundNumber { get; set; }

		/// <summary>
		/// 出价
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public decimal OutPrice { get; set; }

		/// <summary>
		/// 系统售价
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public decimal SysPrice { get; set; }
        /// <summary>
        /// 进价
        /// </summary>
        public decimal BuyPrice { get; set; }
		/// <summary>
		/// 备注
		/// [长度：200]
		/// </summary>
		public string Memo { get; set; }
        /// <summary>
        /// 副条码
        /// </summary>
        public string AssistBarcode { get; set; }
	}
}
