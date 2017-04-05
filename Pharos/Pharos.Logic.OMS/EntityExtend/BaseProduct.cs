// --------------------------------------------------
// Copyright (C) 2016 版权所有
// 创 建 人：蔡少发
// 创建时间：2016-09-03
// 描述信息：
// --------------------------------------------------

using System;

namespace Pharos.Logic.OMS.Entity
{
	/// <summary>
	/// 用于管理本系统的所有产品档案基本信息
	/// </summary>
	[Serializable]
    public abstract class BaseProduct
	{
		/// <summary>
		/// 记录ID
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 信息来源（1-本司2-商户）
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short Source { get; set; }

		/// <summary>
		/// 条形码（全局唯一）
		/// [长度：30]
		/// [不允许为空]
		/// </summary>
		public string Barcode { get; set; }

		/// <summary>
		/// 品名
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 规格
		/// [长度：50]
		/// </summary>
		public string Size { get; set; }

		/// <summary>
		/// 品牌SN
		/// [长度：10]
		/// [默认值：((-1))]
		/// </summary>
		public int BrandSN { get; set; }

		/// <summary>
		/// 品类SN（大类）
		/// [长度：10]
		/// [默认值：((-1))]
		/// </summary>
		public int CategorySN { get; set; }

		/// <summary>
		/// 计量小单位ID（来自数据字典表）
		/// [长度：10]
		/// [默认值：((-1))]
		/// </summary>
		public int SubUnitId { get; set; }

		/// <summary>
		/// 标准价
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public decimal SysPrice { get; set; }

        /// <summary>
        /// 保质期（0:不限）
        /// </summary>
        public short Expiry { get; set; }
        /// <summary>
        /// 保质期单位（1:天、2:月、3:年）
        /// </summary>
        public short? ExpiryUnit { get; set; }

		/// <summary>
		/// 产品状态（0:已下架、1:已上架）
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((1))]
		/// </summary>
		public short State { get; set; }
        /// <summary>
        /// 商户号集合
        /// </summary>
        public string CompanyIds { get; set; }

		/// <summary>
		/// 建档时间
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
		/// </summary>
		public DateTime CreateDT { get; set; }

		/// <summary>
		/// 
		/// [长度：40]
		/// </summary>
		public string CreateUID { get; set; }
	}
}
