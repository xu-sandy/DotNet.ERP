using Newtonsoft.Json;
using Pharos.Logic.Entity;
using Pharos.Sys.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Pharos.Logic.EntityExtend
{
    /// <summary>
    /// 组织结构实体扩展Model
    /// </summary>
    [NotMapped]
    [Serializable]
    //
    public class SysDepartmentsExt:SysDepartments
    {
        public SysDepartmentsExt() { }
        /// <summary>
        /// 子成员数
        /// </summary>
        public int ChildsNum { get; set; }
        /// <summary>
        /// 父级部门名称
        /// </summary>
        public string PTitle { get; set; }
        /// <summary>
        /// 父级类型
        /// </summary>
        public short PType { get; set; }
        /// <summary>
        /// 部门领导名称
        /// </summary>
        public string ManagerUName { get; set; }
        /// <summary>
        /// 部门副领导名称
        /// </summary>
        public string DeputyUName { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 子级权限集合
        /// </summary>
        [JsonProperty("children")]
        public List<SysDepartmentsExt> Childs { get; set; }
    }
}
