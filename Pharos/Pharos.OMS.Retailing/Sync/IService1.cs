using Pharos.Logic.OMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Pharos.OMS.Retailing.Sync
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string GetHasPublish(int cid, int code);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        ProductPublishVer UpdatePublish(int cid, int code, string creater);

        [OperationContract]
        void AddUpdateLog(int publishId, int cid, bool success, string descript, string creater);
    }
}
