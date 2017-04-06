using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Pharos.Sys.Entity;

namespace Pharos.Sys.EntityExtend
{
    /// <summary>
    /// 用于管理本系统的支付配置信息
    /// </summary>
    [NotMapped]
    [Serializable]
    [DataContract(IsReference = true)]
    public class SysPaymentSettingExt:SysPaymentSetting
    {
        [DataMember]
        public string StoreTitle { get; set; }

        [DataMember]
        public string AlterDate { get; set; }

        [DataMember]
        public string StateTitle { get; set; }
        

    }
}
