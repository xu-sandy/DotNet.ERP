using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.ApiPos
{
    /// <summary>
    /// 店员
    /// </summary>
    public class Salesclerk
    {
        public string UID { get; set; }
        public string FullName { get; set; }
        public string UserCode { get; set; }
        public bool Sex { get; set; }
        public string OperateAuth { get; set; }
    }
}
