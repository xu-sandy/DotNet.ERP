// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：黄森
// 创建时间：2015-11-10
// 描述信息：用于管理本系统的所有货品报损基本信息
// --------------------------------------------------

using System;

namespace Pharos.Logic.Entity
{
	/// <summary>
	/// 报损单信息
	/// </summary>
	[Serializable]
    public partial class BreakageGoods:BaseEntity
	{
		/// <summary>
		/// 记录ID
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 报损单Id
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
        public string BreakageGoodsId { get; set; }

		/// <summary>
		/// 报损门店ID
		/// [长度：3]
		/// [不允许为空]
		/// </summary>
		public string StoreId { get; set; }

		/// <summary>
		/// 经办人 UID
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
        public string OperatorUID { get; set; }

		/// <summary>
		/// 登记日期
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
		/// </summary>
		public DateTime CreateDT { get; set; }
        /// <summary>
        /// 已审时间
        /// </summary>
        public DateTime? VerifyTime { get; set; }
        /// <summary>
        /// 报损类别（ 0:出库、 1:入库）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public short BreakageType { get; set; }

        /// <summary>
        /// 原始单据
        /// [长度：40]
        /// </summary>
        public string Receipt { get; set; }

        /// <summary>
        /// 备注
        /// [长度：200]
        /// </summary>
        public string Memo { get; set; }

		/// <summary>
		/// 状态（ 0:未审核、 1:已审核）
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public short State { get; set; }
	}
}
