using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.DataSynchronism.ObjectModels
{
    public class DataSyncResult
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public dynamic Datas { get; set; }

        public dynamic FailDatas { get; set; }
    }
}
