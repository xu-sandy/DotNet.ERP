// --------------------------------------------------
// Copyright (C) 2017 版权所有
// 创 建 人：
// 创建时间：
// 描述信息：
// --------------------------------------------------

using System;

namespace Pharos.Logic.OMS.Entity
{
	/// <summary>
	/// 用于管理本系统的商家支付许可信息
	/// </summary>
	[Serializable]
	public class PayLicense
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
		/// 指派人（SysUser表UserId）
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string DesigneeId
		{
			get { return _DesigneeId; }
			set { _DesigneeId = value; }
		}
		private string _DesigneeId;

		/// <summary>
        /// 所属体系(枚举 :1=云平台；2=外部系统)
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
        public short SourceType
		{
            get { return _SourceType; }
            set { _SourceType = value; }
		}
        private short _SourceType;

		/// <summary>
		/// 服务商号（AgentsInfo表AgentsId）
		/// [长度：10]
		/// </summary>
		public int? AgentsId
		{
			get { return _AgentsId; }
			set { _AgentsId = value; }
		}
		private int? _AgentsId;

		/// <summary>
		/// 省ID
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int ProvinceId
		{
			get { return _ProvinceId; }
			set { _ProvinceId = value; }
		}
		private int _ProvinceId;

		/// <summary>
		/// 市ID
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public int CityId
		{
			get { return _CityId; }
			set { _CityId = value; }
		}
		private int _CityId;

		/// <summary>
		/// 一级经营类目（Business表ById）
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
		public string BusinessId1
		{
			get { return _BusinessId1; }
			set { _BusinessId1 = value; }
		}
		private string _BusinessId1;

		/// <summary>
		/// 二级经营类目（Business表ById）
		/// [长度：40]
		/// </summary>
		public string BusinessId2
		{
			get { return _BusinessId2; }
			set { _BusinessId2 = value; }
		}
		private string _BusinessId2;

		/// <summary>
		/// 企业性质（来自字典）
		/// [长度：10]
		/// </summary>
		public int NatureId
		{
			get { return _NatureId; }
			set { _NatureId = value; }
		}
		private int _NatureId;

		/// <summary>
		/// 商家支付联系人
		/// [长度：20]
		/// [不允许为空]
		/// </summary>
		public string Linkman
		{
			get { return _Linkman; }
			set { _Linkman = value; }
		}
		private string _Linkman;

		/// <summary>
		/// 手机号
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
		/// 常用邮箱
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string Email
		{
			get { return _Email; }
			set { _Email = value; }
		}
		private string _Email;

		/// <summary>
		/// 客服电话
		/// [长度：50]
		/// </summary>
		public string ConsumerHotline
		{
			get { return _ConsumerHotline; }
			set { _ConsumerHotline = value; }
		}
		private string _ConsumerHotline;

		/// <summary>
		/// 资质或证件URL
		/// [长度：200]
		/// [不允许为空]
		/// </summary>
		public string ECertificateUrl1
		{
			get { return _ECertificateUrl1; }
			set { _ECertificateUrl1 = value; }
		}
		private string _ECertificateUrl1;

		/// <summary>
		/// 售卖商品简述
		/// [长度：500]
		/// </summary>
		public string BusinessDescription
		{
			get { return _BusinessDescription; }
			set { _BusinessDescription = value; }
		}
		private string _BusinessDescription;

		/// <summary>
		/// 注册地址
		/// [长度：100]
		/// [不允许为空]
		/// </summary>
		public string RegisterAddress
		{
			get { return _RegisterAddress; }
			set { _RegisterAddress = value; }
		}
		private string _RegisterAddress;

		/// <summary>
		/// 营业执照注册号
		/// [长度：100]
		/// [不允许为空]
		/// </summary>
		public string RegisterNumber
		{
			get { return _RegisterNumber; }
			set { _RegisterNumber = value; }
		}
		private string _RegisterNumber;

		/// <summary>
		/// 经营范围
		/// [长度：300]
		/// [不允许为空]
		/// </summary>
		public string BusinessScope
		{
			get { return _BusinessScope; }
			set { _BusinessScope = value; }
		}
		private string _BusinessScope;

		/// <summary>
		/// 营业期限开始日期
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public string OperatingStartDate
		{
			get { return _OperatingStartDate; }
			set { _OperatingStartDate = value; }
		}
		private string _OperatingStartDate;

		/// <summary>
		/// 营业期限结束日期
		/// [长度：10]
		/// </summary>
		public string OperatingEndDate
		{
			get { return _OperatingEndDate; }
			set { _OperatingEndDate = value; }
		}
		private string _OperatingEndDate;

		/// <summary>
		/// 组织机构代码
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string AgencyCode
		{
			get { return _AgencyCode; }
			set { _AgencyCode = value; }
		}
		private string _AgencyCode;

		/// <summary>
		/// 企业证件URL
		/// [长度：200]
		/// [不允许为空]
		/// </summary>
		public string ECertificateUrl2
		{
			get { return _ECertificateUrl2; }
			set { _ECertificateUrl2 = value; }
		}
		private string _ECertificateUrl2;

		/// <summary>
		/// 企业证件有效期-开始
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public string ECertificateStartDate
		{
			get { return _ECertificateStartDate; }
			set { _ECertificateStartDate = value; }
		}
		private string _ECertificateStartDate;

		/// <summary>
		/// 企业证件有效期-结束
		/// [长度：10]
		/// </summary>
		public string ECertificateEndDate
		{
			get { return _ECertificateEndDate; }
			set { _ECertificateEndDate = value; }
		}
		private string _ECertificateEndDate;

		/// <summary>
		/// 企业法人姓名
		/// [长度：20]
		/// [不允许为空]
		/// </summary>
		public string CorporateName
		{
			get { return _CorporateName; }
			set { _CorporateName = value; }
		}
		private string _CorporateName;

		/// <summary>
		/// 身份证有效期-开始
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public string IDCardStartDate
		{
			get { return _IDCardStartDate; }
			set { _IDCardStartDate = value; }
		}
		private string _IDCardStartDate;

		/// <summary>
		/// 身份证有效期-结算
		/// [长度：10]
		/// </summary>
		public string IDCardEndDate
		{
			get { return _IDCardEndDate; }
			set { _IDCardEndDate = value; }
		}
		private string _IDCardEndDate;

		/// <summary>
		/// 身份证号码
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string IDNumber
		{
			get { return _IDNumber; }
			set { _IDNumber = value; }
		}
		private string _IDNumber;

		/// <summary>
		/// 身份证正面URL
		/// [长度：200]
		/// [不允许为空]
		/// </summary>
		public string IDCardUrl1
		{
			get { return _IDCardUrl1; }
			set { _IDCardUrl1 = value; }
		}
		private string _IDCardUrl1;

		/// <summary>
		/// 身份证反面URL
		/// [长度：200]
		/// [不允许为空]
		/// </summary>
		public string IDCardUrl2
		{
			get { return _IDCardUrl2; }
			set { _IDCardUrl2 = value; }
		}
		private string _IDCardUrl2;

		/// <summary>
		/// 备注
		/// [长度：500]
		/// </summary>
		public string Remark
		{
			get { return _Remark; }
			set { _Remark = value; }
		}
		private string _Remark;

		/// <summary>
        /// 状态（1：未审核，2：被驳回，3：已审核，4暂停，5：注销，6：无效）
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
		public short State
		{
			get { return _State; }
			set { _State = value; }
		}
		private short _State;

		/// <summary>
		/// 后台管理员姓名
		/// [长度：20]
		/// [不允许为空]
		/// </summary>
		public string AdminName
		{
			get { return _AdminName; }
			set { _AdminName = value; }
		}
		private string _AdminName;

		/// <summary>
		/// 支付后台登录账号
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
		public string LoginName
		{
			get { return _LoginName; }
			set { _LoginName = value; }
		}
		private string _LoginName;

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
