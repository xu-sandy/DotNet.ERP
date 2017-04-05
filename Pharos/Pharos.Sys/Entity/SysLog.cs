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
	/// 用于管理本系统的所有日志信息
	/// </summary>
	[Serializable]
    [DataContract(IsReference = true)]
    public class SysLog
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
		/// 类型（1:登录、2:退出、3:普通信息（增删改）、4:异常、5：调试、6:其他）
		/// [长度：3]
		/// </summary>
        [DataMember]
        public byte Type { get; set; }

		/// <summary>
		/// 用户ID
		/// [长度：40]
		/// [不允许为空]
		/// </summary>
        [DataMember]
        public string UId { get; set; }

		/// <summary>
		/// 描述
		/// [长度：-1]
		/// [不允许为空]
		/// </summary>
        [DataMember]
        public string Summary { get; set; }

		/// <summary>
		/// 客户端IP
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
        [DataMember]
        public string ClientIP { get; set; }

		/// <summary>
		/// 服务器名称
		/// [长度：50]
		/// [不允许为空]
		/// </summary>
        [DataMember]
        public string ServerName { get; set; }

		/// <summary>
		/// 记录时间
		/// [长度：23，小数位数：3]
		/// [不允许为空]
		/// [默认值：(getdate())]
		/// </summary>
        [DataMember]
        public DateTime CreateDT { get; set; }
	}
}
