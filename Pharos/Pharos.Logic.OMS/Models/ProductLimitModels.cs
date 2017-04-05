using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Models
{
    public  class LimitModels
    {
        [JsonProperty("children")]
        public virtual List<LimitModels> Childrens { get; set; }
        public int ParentId { get; set; }
        public int? MenuIdFK { get; set; }
        public string MenuTitle { get; set; }
        /// <summary>
        /// 树形涨开或收缩(open|closed)
        /// </summary>
        [JsonProperty("state")]
        public string TreeState { get; set; }
        public int Index { get; set; }
        public int ChildCount { get; set; }
        public int MenuId { get; set; }
        public int LimitId { get; set; }
        public string Title { get; set; }
        public bool Status { get; set; }
        public short? Type { get; set; }
    }
}
