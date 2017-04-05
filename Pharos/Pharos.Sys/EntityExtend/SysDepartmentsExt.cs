﻿// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：ywb
// 创建时间：2015-07-16
// 描述信息：系统菜单信息Model
// --------------------------------------------------

using Newtonsoft.Json;
using Pharos.Sys.Entity;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Pharos.Sys.EntityExtend
{
    /// <summary>
    /// 组织结构实体扩展Model
    /// </summary>
    [NotMapped]
    [Serializable]
    [DataContract(IsReference = true)]
    public class SysDepartmentsExt:SysDepartments
    {
        public SysDepartmentsExt() { }
        /// <summary>
        /// 子成员数
        /// </summary>
        [DataMember]
        public int ChildsNum { get; set; }
        /// <summary>
        /// 父级部门名称
        /// </summary>
        [DataMember]
        public string PTitle { get; set; }
        /// <summary>
        /// 父级类型
        /// </summary>
        [DataMember]
        public short PType { get; set; }
        /// <summary>
        /// 部门领导名称
        /// </summary>
        [DataMember]
        public string ManagerUName { get; set; }
        /// <summary>
        /// 部门副领导名称
        /// </summary>
        [DataMember]
        public string DeputyUName { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        [DataMember]
        public string TypeName { get; set; }
        /// <summary>
        /// 子级权限集合
        /// </summary>
        [DataMember]
        [JsonProperty("children")]
        public List<SysDepartmentsExt> Childs { get; set; }
    }
}
