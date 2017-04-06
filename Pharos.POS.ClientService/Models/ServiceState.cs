using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.POS.ClientService.Models
{
    public class ServiceState
    {
        public string ServiceName { get; set; }
        public string ServiceId { get; set; }
        public bool IsRunning { get; set; }
    }

    public class ServiceOperate
    {
        public string ServiceId { get; set; }
        public ServiceOperateMode ServiceOperateMode { get; set; }
    }

    public enum ServiceOperateMode
    {
        Open = 1,
        Close = 2
    }

    public class ServerState
    {
        public string Project { get; set; }
        public float InUse { get; set; }
    }

    public class ServiceSettings
    {
        public string Redis { get; set; }
        public string WCFIp { get; set; }
        public int WCFPort { get; set; }
        public string SocketIp { get; set; }
        public int SocketPort { get; set; }
    }

    public class StoreSettings
    {
        public int CompanyId { get; set; }
        public string StoreId { get; set; }
        public string RegNo { get; set; }
        public string StoreName { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public string DeviceSn { get; set; }

        public bool ReplaceDatabase { get; set; }
    }
}
