using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
namespace Pharos.Utility
{
    public class TreeModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("parentid")]
        public string ParentId { get; set; }
        [JsonProperty("children")]
        public List<TreeModel> Children { get; set; }
        [JsonProperty("attributes")]
        public object Attributes { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("checked")]
        public bool Checked { get; set; }
    }
}
