using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Data;


namespace Pharos.Logic.BLL.DataSynchronism
{
    public class UpdateFormData
    {

        public UpdateFormData()
        {
            Datas = new Dictionary<string, IEnumerable<object>>();
            Mode = DataSyncMode.FromServerDownload;
        }
        public string StoreId { get; set; }

        /// <summary>
        /// 1、从服务器更新，2、向服务器上传、3、向服务器更新
        /// </summary>
        public DataSyncMode Mode { get; set; }

        public Dictionary<string, IEnumerable<object>> Datas { get; set; }
    }
}
