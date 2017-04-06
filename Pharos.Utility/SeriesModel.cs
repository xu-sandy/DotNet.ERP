using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharos.Utility
{
    /// <summary>
    /// 图形类
    /// </summary>
    public class SeriesModel
    {
        public string name { get; set; }
        public string type { get; set; }
        public string stack { get; set; }
        public List<object> data { get; set; }

        public string radius { get; set; }
        public string[] center { get; set; }

        object _itemStyle = new object();
        public object itemStyle { get { return _itemStyle; } set { _itemStyle = value; } }
    }
}