// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的各类合同基本信息
// --------------------------------------------------

using Pharos.Sys.Extensions;
using System;
using System.Collections.Generic;

namespace Pharos.Logic.Entity
{
	/// <summary>
	/// 合同信息表
	/// </summary>
	[Serializable]
	public partial class Contract:BaseEntity
	{
		/// <summary>
		/// 合同 ID
		/// [主键：√]
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
        [OperationLog("记录ID", false)]
        public string Id { get; set; }

		/// <summary>
		/// 商家 GUID
		/// [长度：40]
		/// </summary>
        [OperationLog("商家ID", false)]
        public string SupplierId { get; set; }

		/// <summary>
		/// 合同分类（ -1:未知）（来自数据字典表）
		/// [长度：10]
		/// [不允许为空]
		/// [默认值：((-1))]
		/// </summary>
        [OperationLog("合同分类", false)]
        public int ClassifyId { get; set; }

		/// <summary>
		/// 合同编号
		/// [长度：20]
		/// [不允许为空]
		/// </summary>
        [OperationLog("合同编号", false)]
        public string ContractSN { get; set; }

		/// <summary>
		/// 合同名称
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
        [OperationLog("合同名称", false)]
        public string Title { get; set; }

		/// <summary>
		/// 父合同编号（为空表示新签，非空表示续签）
		/// [长度：40]
		/// </summary>
        [OperationLog("父合同编号", false)]
        public string PId { get; set; }

		/// <summary>
		/// 版本
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((1))]
		/// </summary>
        public short Version { get { return _Version; } set { _Version = value; } }
        short _Version = 1;

		/// <summary>
		/// 签订日期
		/// [长度：10]
		/// </summary>
        [OperationLog("签订日期", false)]
        public string SigningDate { get; set; }

		/// <summary>
		/// 生效日期
		/// [长度：10]
		/// </summary>
        [OperationLog("生效日期", false)]
        public string StartDate { get; set; }

		/// <summary>
		/// 结束日期
		/// [长度：10]
		/// </summary>
        [OperationLog("结束日期", false)]
        public string EndDate { get; set; }

		/// <summary>
		/// 创建人（ UID）
		/// [长度：40]
		/// [不允许为空]
		/// [默认值：((-1))]
		/// </summary>
        [Pharos.Utility.Exclude]
        [OperationLog("创建人", false)]
        public string CreateUID { get; set; }

		/// <summary>
		/// 创建时间
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
		/// </summary>
        [Pharos.Utility.Exclude]
        [OperationLog("创建时间", false)]
        public DateTime CreateDT { get; set; }

		/// <summary>
        /// 合同状态（0：未提交、1：待审批、2：已审批、3：已中止、4：已结束）
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
        [Pharos.Utility.Exclude]
        [OperationLog("合同状态",  "0:未提交","1:待审批", "2:已审批", "3:已中止", "4:已结束")]
        public short State { get; set; }

	}
}


