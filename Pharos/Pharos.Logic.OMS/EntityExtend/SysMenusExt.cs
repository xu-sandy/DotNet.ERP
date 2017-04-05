// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：
// 创建时间：2015-07-16
// 描述信息：系统菜单信息Model
// --------------------------------------------------

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Pharos.Logic.OMS.Entity;

namespace Pharos.Logic.OMS.EntityExtend
{
    /// <summary>
    /// 菜单信息
    /// </summary>
    [NotMapped]
    [Serializable]
    [DataContract(IsReference = true)]
    public class SysMenusExt : SysMenus
    {
        /// <summary>
        /// 父级菜单名称
        /// </summary>
        [DataMember]
        public string PTitle { get; set; }

        /// <summary>
        /// 子级菜单集合
        /// </summary>
        [DataMember]
        [JsonProperty("children")]
        public List<SysMenusExt> Childs { get; set; }
    }
}
