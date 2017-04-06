using Pharos.Logic.OMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Pharos.OMS.Retailing.Sync
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service1.svc 或 Service1.svc.cs，然后开始调试。
    public class Service1 : IService1
    {
        [Ninject.Inject]
        Pharos.Logic.OMS.BLL.ProductPublishVerService src { get; set; }

        [OperationBehavior(TransactionScopeRequired = true)]//,TransactionAutoComplete = true)]
        public ProductPublishVer UpdatePublish(int cid, int code, string creater)
        {
            return src.UpdatePublish(cid, code, creater);
        }
        public void AddUpdateLog(int publishId, int cid, bool success, string descript, string creater)
        {
            src.AddUpdateLog(publishId, cid, success, descript, creater);
        }

        public string GetHasPublish(int cid, int code)
        {
            return src.GetHasPublish(cid, code);
        }
    }
}
