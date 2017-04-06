// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-07-24
// 描述信息：用于管理本系统的所有货品入库的明细信息
// --------------------------------------------------

using System;

namespace Pharos.Logic.Entity
{
	/// <summary>
	/// 入库明细信息
	/// </summary>
	[Serializable]
	public partial class InboundList
	{
		/// <summary>
		/// 明细ID
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
        public int Id { get; set; }

		/// <summary>
		/// 入库单ID
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string InboundGoodsId { get; set; }

		/// <summary>
		/// 条码
		/// [长度：30]
		/// [不允许为空]
		/// </summary>
		public string Barcode { get; set; }

		/// <summary>
		/// 入库数量
		/// [不允许为空]
		/// </summary>
        public decimal InboundNumber { get; set; }

		/// <summary>
		/// 进价
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public decimal BuyPrice { get; set; }

		/// <summary>
		/// 系统售价
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public decimal SysPrice { get; set; }

		/// <summary>
		/// 备注
		/// [长度：200]
		/// </summary>
		public string Memo { get; set; }

		/// <summary>
		/// 状态（0：待验，1：已验）
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public short State { get; set; }
        
        /// <summary>
        /// 生产日期
        /// </summary>
        public string ProducedDate { get; set; }

        /// <summary>
        /// 有效期（天数）
        /// </summary>
        public short ExpiryDate { get; set; }

        /// <summary>
        /// 有效期单位（1：天、2：月、3、年）
        /// </summary>
        public short? ExpiryDateUnit { get; set; }

        /// <summary>
        /// 截止保质日期
        /// </summary>
        public string ExpirationDate { get; set; }

        /// <summary>
        /// 是否为赠品（0：否，1：是）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public short IsGift { get; set; }

        /// <summary>
        /// 副条码
        /// </summary>
        public string AssistBarcode { get; set; }
	}
}
