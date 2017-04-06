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
    /// 用于管理本系统的分机构、部门信息
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public class SysDepartments : BaseEntity
    {
        /// <summary>
        /// 类型（1:机构、2:部门、3:子部门）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((2))]
        /// </summary>
        [DataMember]
        public short Type { get; set; }

        /// <summary>
        /// 机构部门Id(全局唯一)
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        [DataMember]
        public int DepId { get; set; }

        /// <summary>
        /// 隶属分机构ID（0:顶级）
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [DataMember]
        public int PDepId { get; set; }

        /// <summary>
        /// 排序（0:无）
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [DataMember]
        public int SortOrder { get; set; }

        /// <summary>
        /// 部门名称
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 部门代码
        /// [长度：50]
        /// </summary>
        [DataMember]
        public string SN { get; set; }

        /// <summary>
        /// 部门领导ID（-1:未知）
        /// [长度：40]
        /// [默认值：((-1))]
        /// </summary>
        [DataMember]
        public string ManagerUId { get; set; }

        /// <summary>
        /// 部门副领导ID（-1:未知）
        /// [长度：40]
        /// [默认值：((-1))]
        /// </summary>
        [DataMember]
        public string DeputyUId { get; set; }

        /// <summary>
        /// 登录后首页
        /// [长度：200]
        /// </summary>
        [DataMember]
        public string IndexPageUrl { get; set; }

        /// <summary>
        /// 状态（0:关闭、1:显示）
        /// [长度：1]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        [DataMember]
        public bool Status { get; set; }
    }
}
