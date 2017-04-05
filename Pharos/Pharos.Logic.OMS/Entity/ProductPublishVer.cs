// --------------------------------------------------
// Copyright (C) 2017 版权所有
// 创 建 人：
// 创建时间：2017-02-14
// 描述信息：
// --------------------------------------------------

using System;

namespace Pharos.Logic.OMS.Entity
{
	/// <summary>
	/// 升级发布
	/// </summary>
	[Serializable]
	public partial class ProductPublishVer
	{
		/// <summary>
		/// 
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 发布编号
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int PublishId { get; set; }

		/// <summary>
		/// 产品编号
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int ProductId { get; set; }

		/// <summary>
		/// 初始功能编号
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public string ModuleId { get; set; }

		/// <summary>
        /// 初始角色编号
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
        public string RoleId { get; set; }

		/// <summary>
        /// 初始字典编号
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
        public string DictId { get; set; }

		/// <summary>
		/// 初始数据编号
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
        public string DataId { get; set; }
        /// <summary>
        /// 处理历史数据关联菜单
        /// </summary>
        public string MenuModuleId { get; set; }
        /// <summary>
        /// 处理SQL方式(1-分块处理，2-统一处理)
        /// </summary>
        public short RunSqlWay { get; set; }
		/// <summary>
		/// 处理历史数据
		/// [长度：1]
		/// [不允许为空]
		/// </summary>
		public bool IsRunSql { get; set; }

		/// <summary>
		/// 状态（0-未生效，1-已生效，2-已失效）
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short Status { get; set; }

		/// <summary>
		/// 版本状态（0-未发布，1-测试版，2-正式版）
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short VerStatus { get; set; }

		/// <summary>
		/// 版本号
		/// [长度：19，小数位数：4]
		/// [不允许为空]
		/// </summary>
		public decimal VerCode { get; set; }

		/// <summary>
		/// 是否强制升级
		/// [长度：1]
		/// [不允许为空]
		/// </summary>
		public bool Forced { get; set; }

		/// <summary>
		/// 影响商户数
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public int CompanyCount { get; set; }

		/// <summary>
		/// 
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
		/// </summary>
		public DateTime UpdateDT { get; set; }

		/// <summary>
		/// 
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string UpdateUID { get; set; }

		/// <summary>
		/// 
		/// [长度：23，小数位数：3]
		/// </summary>
		public DateTime? PublishDT { get; set; }

		/// <summary>
		/// 
		/// [长度：40]
		/// </summary>
		public string PublishUID { get; set; }

		/// <summary>
		/// 
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
		/// </summary>
		public DateTime CreateDT { get; set; }

		/// <summary>
		/// 
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string CreateUID { get; set; }
	}
}
