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
	/// 
	/// </summary>
	[Serializable]
	public class Traders
	{
		/// <summary>
		/// 记录 ID
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 企业 ID（ 全局唯一， 自动生成）
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int CID { get; set; }

		/// <summary>
		/// 分类 GUID
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string TraderTypeId { get; set; }

		/// <summary>
		/// 信息来源
		/// [长度：20]
		/// [不允许为空]
		/// </summary>
		public string Source { get; set; }

		/// <summary>
		/// 简称
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 全称
		/// [长度：100]
		/// [不允许为空]
		/// </summary>
		public string FullTitle { get; set; }

		/// <summary>
		/// 省份 ID
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short CurrentProvinceId { get; set; }

		/// <summary>
		/// 城市 ID
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short CurrentCityId { get; set; }

        /// <summary>
        /// 区县 ID
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short CurrentCounty { get; set; }

		/// <summary>
		/// 详细地址
		/// [长度：200]
		/// </summary>
		public string Address { get; set; }

		/// <summary>
		/// 联系人
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string LinkMan { get; set; }

		/// <summary>
		/// 手机号码
		/// [长度：11]
		/// [不允许为空]
		/// </summary>
		public string MobilePhone { get; set; }

		/// <summary>
		/// 经营模式(取字典表)
		/// [长度：5]
		/// </summary>
		public short BusinessModeId { get; set; }

		/// <summary>
		/// 经营类目(取行业类别 ID，多个以逗号隔开)
		/// [长度：200]
		/// </summary>
		public string BusinessScopeId { get; set; }

		/// <summary>
		/// 货品盘点情况
		/// [长度：200]
		/// </summary>
		public string TakeStockDates { get; set; }

		/// <summary>
		/// 现有系统名称
		/// [长度：50]
		/// </summary>
		public string ExistsystemName { get; set; }

		/// <summary>
		/// 现在设备名称
		/// [长度：50]
		/// </summary>
		public string ExistDeviceName { get; set; }

		/// <summary>
		/// 现有门店数量（来自字典）
		/// [长度：5]
		/// </summary>
		public int ExistStoreNum { get; set; }

		/// <summary>
		/// 每个门店机数
		/// [长度：5]
		/// </summary>
		public short EachStorePosNum { get; set; }

		/// <summary>
		/// 每个门店人均数（来自字典）
		/// [长度：5]
		/// </summary>
		public int EachStorePersonNum { get; set; }

		/// <summary>
		/// 计划扩张门店数量
		/// [长度：5]
		/// </summary>
		public int PlanExpandStoreNum { get; set; }

		/// <summary>
		/// 跟踪状态 ID（来自
		/// [长度：5]
		/// </summary>
		public short TrackStautsId { get; set; }

		/// <summary>
		/// 登记人 UID
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string CreateUID { get; set; }

		/// <summary>
		/// 创建时间
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
		/// </summary>
		public DateTime CreateDT { get; set; }

		/// <summary>
		/// 更新时间
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
		/// </summary>
		public DateTime UpdateDT { get; set; }

		/// <summary>
		/// 备注
		/// [长度：1000]
		/// </summary>
		public string Memo { get; set; }

		/// <summary>
		/// 指派人 UID
		/// [长度：40]
		/// </summary>
		public string AssignerUID { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string Assigner { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string Cities { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string CityTitles { get; set; }
		/// <summary>
		/// 状态（ 0：未审核、1：已审核、 2:无效）
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
		public short Status { get; set; }

        /// <summary>
        /// 支付方式（来自字典，逗号隔开）
        /// [长度：1000]
        /// </summary>
        public string Pay { get; set; }

        /// <summary>
        /// 竞争对手
        /// [长度：200]
        /// </summary>
        public string Rival { get; set; }

        /// <summary>
        /// 竞争对手的营销模式
        /// [长度：200]
        /// </summary>
        public string Marketing { get; set; }

        /// <summary>
        /// 0是电脑添加，1是手机添加
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int AddType { get; set; }
	}
}
