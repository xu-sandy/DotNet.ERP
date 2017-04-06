// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：
// 创建时间：2015-07-29
// 描述信息：
// --------------------------------------------------

using Pharos.Utility;
using System;
using System.Runtime.Serialization;

namespace Pharos.Sys.Entity
{
	/// <summary>
	/// 用于管理本系统的用户自定义权限信息
	/// </summary>
	[Serializable]
    [DataContract(IsReference = true)]
    public class SysUsersLimits
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
        public string UId { get; set; }

		/// <summary>
		/// 权限ID
		/// [长度：10]
		/// [不允许为空]
		/// </summary>
        [DataMember]
        public int LimitsCode { get; set; }
	}
}
