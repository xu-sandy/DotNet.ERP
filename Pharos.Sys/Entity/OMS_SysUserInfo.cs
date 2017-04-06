// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：
// 创建时间：2015-07-29
// 描述信息：
// --------------------------------------------------

using Pharos.Utility;
using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharos.Sys.Entity
{
	/// <summary>
	/// 用于管理本系统的用户基本信息
	/// </summary>
	[Serializable]
    [DataContract(IsReference = true)]
    public class OMS_SysUserInfo
	{
		/// <summary>
		/// 记录ID
		/// [主键：√]
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
        [DataMember]
        public int Id { get; set; }

		/// <summary>
		/// 信息创建者ID
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
        [DataMember]
        public string UID { get; set; }

		/// <summary>
		/// 
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
        [DataMember]
        public string UserCode { get; set; }

		/// <summary>
		/// 姓名
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
        [DataMember]
        public string FullName { get; set; }

		/// <summary>
		/// 登录账号
		/// [长度：100]
		/// [不允许为空]
		/// </summary>
        [DataMember]
        public string LoginName { get; set; }

		/// <summary>
		/// 登录密钥
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
        [DataMember]
        public string LoginPwd { get; set; }

		/// <summary>
		/// 性别（0:女、1:男）
		/// [长度：1]
		/// [默认值：((1))]
		/// </summary>
        [DataMember]
        public bool Sex { get { return _Sex; } set { _Sex = value; } }
        bool _Sex = true;
		/// <summary>
		/// 隶属机构ID
		/// [长度：10]
		/// </summary>
        [DataMember]
        public int BranchId { get; set; }

		/// <summary>
		/// 隶属部门ID
        /// [长度：10]
		/// </summary>
        [DataMember]
        public int BumenId { get; set; }

		/// <summary>
		/// 直属上司ID
        /// [长度：40]
		/// </summary>
        [DataMember]
        public string BossUId { get; set; }

		/// <summary>
        /// 担任职位(来自数据字典)
        /// [长度：10]
		/// </summary>
        [DataMember]
        public int PositionId { get; set; }

		/// <summary>
		/// 头像URL
		/// [长度：200]
		/// </summary>
        [DataMember]
        public string PhotoUrl { get; set; }

		/// <summary>
		/// 状态（1:正常、2:锁定、3:注销）
		/// [长度：5]
		/// [不允许为空]
		/// [默认值：((1))]
		/// </summary>
        [DataMember]
        public short Status { get; set; }

		/// <summary>
		/// 隶属角色组ID（多个间用,逗号间隔）
		/// [长度：2000]
		/// </summary>
        [DataMember]
        public string RoleIds { get; set; }

		/// <summary>
		/// 最近登录IP
		/// [长度：50]
		/// </summary>
        [DataMember]
        public string LoginIP { get; set; }

		/// <summary>
		/// 最近登录时间
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
		/// </summary>
        [DataMember]
        public DateTime LoginDT { get; set; }

		/// <summary>
		/// 登录次数（0:未登录过）
		/// [长度：10]
		/// [不允许为空]
		/// [默认值：((0))]
		/// </summary>
        [DataMember]
        public int LoginNum { get; set; }

		/// <summary>
		/// 
		/// [长度：40]
		/// </summary>
        [NotMapped]
        public string StoreId { get; set; }

		/// <summary>
		/// 创建时间
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
		/// </summary>
        [DataMember]
        public DateTime CreateDT { get; set; }

		/// <summary>
		/// 信息创建者 ID
		/// [长度：40]
		/// </summary>
        public string CreateUID { get; set; }
        /// <summary>
        /// 个性签名
        /// </summary>
        [DataMember]
        public string Signature { get; set; }
        /// <summary>
        /// 手势密码
        /// </summary>
        [DataMember]
        public string Handsign { get; set; }
        /// <summary>
        /// 手机号
        /// [长度：20]
        /// </summary>
        [DataMember]
        public string Mobile { get; set; }
        /// <summary>
        /// 环信IM用户uuid
        /// [长度：50]
        /// </summary>
        [DataMember]
        public string HuanXinUUID { get; set; }
	}
}