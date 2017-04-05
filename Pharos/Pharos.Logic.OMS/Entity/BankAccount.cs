// --------------------------------------------------
// Copyright (C) 2017 版权所有
// 创 建 人：
// 创建时间：
// 描述信息：
// --------------------------------------------------

using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Pharos.Logic.OMS.Entity
{
	/// <summary>
	/// 用于管理本系统的商家结算账户信息
	/// </summary>
	[Serializable]
	public class BankAccount
	{
		/// <summary>
		/// 记录ID
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int Id
		{
			get { return _Id; }
			set { _Id = value; }
		}
		private int _Id;

		/// <summary>
		/// 结算账户编号
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string BAccountId
		{
			get { return _BAccountId; }
			set { _BAccountId = value; }
		}
		private string _BAccountId;

		/// <summary>
		/// 支付许可编号
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string LicenseId
		{
			get { return _LicenseId; }
			set { _LicenseId = value; }
		}
		private string _LicenseId;

		/// <summary>
		/// 商户号（Traders表CID）
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int? CID
		{
			get { return _CID; }
			set { _CID = value; }
		}
		private int? _CID;

		/// <summary>
		/// 财务联系人
		/// [长度：20]
		/// [不允许为空]
		/// </summary>
		public string LinkMan
		{
			get { return _LinkMan; }
			set { _LinkMan = value; }
		}
		private string _LinkMan;

		/// <summary>
		/// 联系人电话
		/// [长度：20]
		/// [不允许为空]
		/// </summary>
		public string Phone
		{
			get { return _Phone; }
			set { _Phone = value; }
		}
		private string _Phone;

		/// <summary>
		/// 账户类型（1:对公，2:私人）
		/// [长度：5]
		/// [不允许为空]
		/// </summary>
		public short AccountType
		{
			get { return _AccountType; }
			set { _AccountType = value; }
		}
		private short _AccountType;

		/// <summary>
		/// 开户名称
		/// [长度：100]
		/// [不允许为空]
		/// </summary>
		public string AccountName
		{
			get { return _AccountName; }
			set { _AccountName = value; }
		}
		private string _AccountName;

		/// <summary>
		/// 开户银行城市（Area表AreaID）
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int BankCityId
		{
			get { return _BankCityId; }
			set { _BankCityId = value; }
		}
		private int _BankCityId;

        /// <summary>
        /// 开户银行城市名称
        /// </summary>
        [NotMapped]
        public string BankCityName { get; set; }

		/// <summary>
		/// 开户银行
		/// [长度：100]
		/// [不允许为空]
		/// </summary>
		public string BankName
		{
			get { return _BankName; }
			set { _BankName = value; }
		}
		private string _BankName;

		/// <summary>
		/// 开户支行
		/// [长度：100]
		/// </summary>
		public string BranchName
		{
			get { return _BranchName; }
			set { _BranchName = value; }
		}
		private string _BranchName;

		/// <summary>
		/// 银行账号
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string AccountNumber
		{
			get { return _AccountNumber; }
			set { _AccountNumber = value; }
		}
		private string _AccountNumber;

		/// <summary>
        /// 账户状态（1:未审核，2:可用，3:被驳回，4:暂停，5:注销，6:无效）
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int State
		{
			get { return _State; }
			set { _State = value; }
		}
		private int _State;

		/// <summary>
		/// 审核人（SysUser表UserId）
		/// [长度：40]
		/// </summary>
		public string AuditUID
		{
			get { return _AuditUID; }
			set { _AuditUID = value; }
		}
		private string _AuditUID;

		/// <summary>
		/// 审核时间
		/// [长度：23，小数位数：3]
		/// </summary>
		public DateTime? AuditDT
		{
			get { return _AuditDT; }
			set { _AuditDT = value; }
		}
		private DateTime? _AuditDT;

		/// <summary>
		/// 创建人（SysUser表UserId）
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string CreateUID
		{
			get { return _CreateUID; }
			set { _CreateUID = value; }
		}
		private string _CreateUID;

		/// <summary>
		/// 创建时间
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
		/// </summary>
		public DateTime CreateDT
		{
			get { return _CreateDT; }
			set { _CreateDT = value; }
		}
		private DateTime _CreateDT;

        /// <summary>
        /// 修改人（SysUser表UserId）
        /// </summary>
        public string ModifyUID { get; set; }

		/// <summary>
		/// 修改时间
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
		/// </summary>
		public DateTime ModifyDT
		{
			get { return _ModifyDT; }
			set { _ModifyDT = value; }
		}
		private DateTime _ModifyDT;
	}
}
