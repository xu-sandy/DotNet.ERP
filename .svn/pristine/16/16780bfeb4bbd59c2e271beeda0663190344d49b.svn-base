using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Models
{
    public class TreeModel
    {
        [JsonProperty("children")]
        public virtual List<TreeModel> Childrens { get; set; }
        /// <summary>
        /// 树形涨开或收缩(open|closed)
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
