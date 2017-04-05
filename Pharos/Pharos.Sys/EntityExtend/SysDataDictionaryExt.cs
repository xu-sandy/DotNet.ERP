using Pharos.Sys.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.Sys.EntityExtend
{
    [NotMapped]
    [Serializable]
    [DataContract(IsReference = true)]
    public class SysDataDictionaryExt : SysDataDictionary
    {
        public SysDataDictionaryExt() { }
        /// <summary>
        /// 显示的子项字符串
        /// </summary>
        [DataMember]
        public string ItemsStr { get; set; }
        /// <summary>
        /// 所有子项树
        /// </summary>
        [DataMember]
        public int ItemsCount { get; set; }
        /// <summary>
        /// 父级字典名称
        /// </summary>
        [DataMember]
        public string PTitle { get; set; }
    }
}
