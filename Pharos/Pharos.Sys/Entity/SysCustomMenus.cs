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
    /// 用于管理本系统的部门或用户自定义菜单配置信息
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public class SysCustomMenus : BaseEntity
    {
        /// <summary>
        /// 记录ID
        /// [主键：√]
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        //[DataMember]
        //public int Id { get; set; }

        /// <summary>
        /// 适用类型（-1:全部,1:部门,2:角色,3:用户）
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((-1))]
        /// </summary>
        [DataMember]
        public int Type { get; set; }

        /// <summary>
        /// 适用对象ID（-1:全部,部门ID、角色ID、用户ID）
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((-1))]
        /// </summary>
        [DataMember]
        public int ObjId { get; set; }

        /// <summary>
        /// 拥有菜单项（多个间用,号间隔，-1:继承所在部门）（来自SysMenus表Id）
        /// [长度：-1]
        /// [不允许为空]
        /// [默认值：((-1))]
        /// </summary>
        [DataMember]
        public string MenuIds { get; set; }
    }
}
