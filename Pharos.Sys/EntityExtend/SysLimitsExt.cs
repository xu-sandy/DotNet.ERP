// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：ywb
// 创建时间：2015-07-16
// 描述信息：系统菜单信息Model
// --------------------------------------------------

using Newtonsoft.Json;
using Pharos.Sys.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Pharos.Sys.EntityExtend
{
    /// <summary>
    /// 权限实体扩展Model
    /// </summary>
    [NotMapped]
    [Serializable]
    [DataContract(IsReference = true)]
    public class SysLimitsExt : SysLimits
    {
        /// <summary>
        /// 父级权限名称
        /// </summary>
        [DataMember]
        public string PTitle { get; set; }
        /// <summary>
        /// 父级深度
        /// </summary>
        [DataMember]
        public int PDepth { get; set; }
        /// <summary>
        /// 权限子集字符串
        /// </summary>
        [DataMember]
        public string ItemsStr { get; set; }
        /// <summary>
        /// 子级权限集合
        /// </summary>
        [DataMember]
        [JsonProperty("children")]
        public List<SysLimitsExt> Childs { get; set; }
        [DataMember]
        public bool HasChild { get; set; }

        /// <summary>
        /// 子级权限集合
        /// </summary>
        [DataMember]
        [JsonProperty("LastChildren")]
        public List<SysLimitsExt> LastChilds { get; set; }

    }
}
