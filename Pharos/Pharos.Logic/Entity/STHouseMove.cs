// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的所有门店的货品调拨信息
// --------------------------------------------------

using Newtonsoft.Json;
using Pharos.Utility;
using System;

namespace Pharos.Logic.Entity
{
	/// <summary>
	/// 货品调拨信息
	/// </summary>
	[Serializable]
	public partial class STHouseMove
	{
		/// <summary>
		/// 调拨ID
		/// [主键：√]
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// 调出分店ID
		/// [长度：3]
		/// [不允许为空]
		/// </summary>
		public string OutStoreId { get; set; }

		/// <summary>
		/// 调入分店ID
		/// [长度：3]
		/// [不允许为空]
		/// </summary>
		public string InStoreId { get; set; }

		/// <summary>
		/// 调拨商品条码
		/// [长度：30]
		/// [不允许为空]
		/// </summary>
		public string Barcode { get; set; }

		/// <summary>
		/// 申请数量
		/// [不允许为空]
		/// </summary>
        public decimal OrderQuantity { get; set; }

		/// <summary>
		/// 申请时间
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
		/// </summary>
        [JsonConverter(typeof(JsonShortDate))]
		public DateTime CreateDT { get; set; }

		/// <summary>
		/// 申请人UID
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string CreateUID { get; set; }

		/// <summary>
		/// 配送数量	
		/// [默认值：((0))]
		/// </summary>
        public decimal DeliveryQuantity { get; set; }

		/// <summary>
		/// 配送人UID
		/// [长度：40]
		/// </summary>
		public string DeliveryUID { get; set; }

		/// <summary>
		/// 收货数量
		/// [默认值：((0))]
		/// </summary>
        public decimal ActualQuantity { get; set; }

		/// <summary>
		/// 收货人UID
		/// [长度：40]
		/// </summary>
		public string ActualUID { get; set; }

		/// <summary>
		/// 备注
		/// [长度：200]
		/// </summary>
		public string Memo { get; set; }

		/// <summary>
		/// 状态（1:调拨中、2:已配送、3:已撤回、4:已收货）
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((1))]
		/// </summary>
		public short State { get; set; }

		/// <summary>
		/// 退回请求原因
		/// [长度：10]
		/// </summary>
		public int? ReasonId { get; set; }

        public short StockOut { get; set; }
	}
}
