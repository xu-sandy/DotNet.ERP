// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的各类合同签约双方基本信息
// --------------------------------------------------

using Pharos.Sys.Extensions;
using System;

namespace Pharos.Logic.Entity
{
	/// <summary>
	/// 合同双方信息
	/// </summary>
	[Serializable]
	public class ContractBoth
	{
        /// <summary>
        /// 记录ID
        /// </summary>
        [OperationLog("记录ID", false)]
        public int Id { get; set; }
		/// <summary>
		/// 主合同 ID（来自Contract 表）
		/// [主键：√]
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
        [Pharos.Utility.Exclude]
        public string ContractId { get; set; }

		/// <summary>
		/// 签约方（ 1:甲方、2:乙方、 3:丙方）
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
        [OperationLog("签约方", "1:甲方", "2:乙方", "3:丙方")]
        public short Signatory { get; set; }

		/// <summary>
		/// 公司名称
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
        [OperationLog("公司名称", false)]
        public string CompanyName { get; set; }

		/// <summary>
		/// 签约代表
		/// [长度：20]
		/// [不允许为空]
		/// </summary>
        [OperationLog("签约代表", false)]
        public string Representative { get; set; }

		/// <summary>
		/// 电话
		/// [长度：20]
		/// </summary>
        [OperationLog("电话", false)]
        public string Tel { get; set; }

		/// <summary>
		/// 传真
		/// [长度：20]
		/// </summary>
        [OperationLog("传真", false)]
        public string Fax { get; set; }

		/// <summary>
		/// 税号
		/// [长度：20]
		/// </summary>
        [OperationLog("税号", false)]
        public string TaxNumber { get; set; }

		/// <summary>
		/// 网址
		/// [长度：200]
		/// </summary>
        [OperationLog("网址", false)]
        public string Url { get; set; }

		/// <summary>
		/// 地址
		/// [长度：100]
		/// </summary>
        [OperationLog("地址", false)]
        public string Address { get; set; }

		/// <summary>
		/// 邮编
		/// [长度：6]
		/// </summary>
        [OperationLog("邮编", false)]
        public string PostCode { get; set; }

		/// <summary>
		/// 支付行号
		/// [长度：20]
		/// </summary>
        [OperationLog("支付行号", false)]
        public string PayNumber { get; set; }

		/// <summary>
		/// 开户银行
		/// [长度：50]
		/// </summary>
        [OperationLog("开户银行", false)]
        public string BankName { get; set; }

		/// <summary>
		/// 银行账号
		/// [长度：20]
		/// </summary>
        [OperationLog("银行账号", false)]
        public string BankAccount { get; set; }
        [Pharos.Utility.Exclude]
        public Contract Contract { get; set; }
	}
}
