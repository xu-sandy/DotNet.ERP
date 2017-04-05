// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-08-03
// 描述信息：用于管理本门店的店员基本信息
// --------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Pharos.Utility;

namespace Pharos.Sys.Entity
{
    /// <summary>
    /// 店员信息
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    [Excel("店员信息表")]
    public class SysStoreUserInfo
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
        /// 用户ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [DataMember]
        [Excel("用户ID", 1)]
        public string UID { get; set; }

        /// <summary>
        /// 姓名
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        [DataMember]
        [Excel("姓名", 2)]
        public string FullName { get; set; }

        /// <summary>
        /// 员工编号
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        [DataMember]
        [Excel("员工编号", 3)]
        public string UserCode { get; set; }
        /// <summary>
        /// 员工编号
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        [DataMember]
        [Excel("登录帐号", 4)]
        public string LoginName { get; set; }
        /// <summary>
        /// 登录密钥
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        [DataMember]
        [Excel("登录密钥", 5)]
        public string LoginPwd { get; set; }

        /// <summary>
        /// 性别（0:女、1:男）
        /// [长度：1]
        /// [默认值：((1))]
        /// </summary>
        [DataMember]
        [Excel("性别", 6)]
        public bool Sex { get; set; }

        /// <summary>
        /// 本店角色（1:店长、2:营业员、3:收银员、4:数据维护），格式：门店ID,角色ID|门店ID,角色ID
        /// [长度：2000]
        /// [不允许为空]
        /// </summary>
        [DataMember]
        [Excel("本店角色", 7)]
        public string OperateAuth { get; set; }

        /// <summary>
        /// 状态（1:正常、2:锁定、3:注销）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        [DataMember]
        public short Status { get; set; }

        /// <summary>
        /// 最近登录时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        [DataMember]
        [Excel("最近登录时间", 8)]
        public DateTime LoginDT { get; set; }

        /// <summary>
        /// 创建时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        [DataMember]
        [Excel("创建时间", 9)]
        public DateTime CreateDT { get; set; }

        [DataMember]
        public int CompanyId { get; set; }
        [DataMember]
        [Pharos.Utility.Exclude]
        public byte[] SyncItemVersion { get; set; }
        [DataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SyncItemId { get; set; }
        [NotMapped]
        [DataMember]
        public string Stroes { get; set; }
        [NotMapped]
        public string RoleIds { get; set; }
    }
}
