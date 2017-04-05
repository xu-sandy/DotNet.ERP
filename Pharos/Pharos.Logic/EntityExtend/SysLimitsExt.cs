using Newtonsoft.Json;
using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Pharos.Logic.EntityExtend
{
    /// <summary>
    /// 权限实体扩展Model
    /// </summary>
    [NotMapped]
    [Serializable]
    //
    public class SysLimitsExt:SysLimits
    {
        public SysLimitsExt() { }
        public SysLimitsExt(SysLimits limit)
        {
            Id = limit.Id;
            PId = limit.PId;
            Key = limit.Key;
            Code = limit.Code;
            Depth = limit.Depth;
            Status = limit.Status;
        }

        /// <summary>
        /// 父级权限名称
        /// </summary>
        public string PKey { get; set; }
        /// <summary>
        /// 父级深度
        /// </summary>
        public int PDepth { get; set; }
        /// <summary>
        /// 权限子集字符串
        /// </summary>
        public string ChildLimitsStr { get; set; }
        /// <summary>
        /// 子级权限集合
        /// </summary>
        [JsonProperty("children")]
        public List<SysLimitsExt> Childs { get; set; }
    }
}
