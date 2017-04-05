// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：
// 创建时间：2015-07-29
// 描述信息：
// --------------------------------------------------

using Pharos.Utility;
using System;
using System.Runtime.Serialization;

namespace Pharos.Logic.OMS.Entity
{
    /// <summary>
    /// 用于管理本系统的菜单基本信息
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public class SysMenus
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
        /// 菜单 Id （全局唯一）
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((-1))]
        /// </summary>
         [DataMember]
        public int MenuId { get; set; }

        /// <summary>
        /// 上级菜单（ 0:顶级）
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((-1))]
        /// </summary>
         [DataMember]
        public int PMenuId { get; set; }

        /// <summary>
        /// 排序
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
         [DataMember]
        public int SortOrder { get; set; }

        /// <summary>
        /// 名称
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
         [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 
        /// [长度：200]
        /// [不允许为空]
        /// </summary>
         [DataMember]
        public string URL { get; set; }

        /// <summary>
        /// 状态（0:隐藏、1:显示）
        /// [长度：1]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
         [DataMember]
        public bool Status { get; set; }
    }
}
