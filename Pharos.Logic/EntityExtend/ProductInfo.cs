using Pharos.Logic.BLL.LocalServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Pharos.Logic.LocalEntity
{
    public partial class ProductInfo
    {
        /// <summary>
        /// 计量大单位（来自数据字典表） 
        /// [长度：50] 
        /// </summary>
        [NotMapped]
        public string BigUnit
        {
            get
            {
                var unit = SysDataDictionaryLocalService.Find(o => o.DicSN == this.BigUnitId);
                return unit != null ? unit.Title : "未知单位";
            }
        }

        /// <summary>
        /// 计量小单位（来自数据字典表） 
        /// [长度：50]
        /// </summary>
        [NotMapped]
        public string SubUnit
        {
            get
            {
                var unit = SysDataDictionaryLocalService.Find(o => o.DicSN == this.SubUnitId);
                return unit != null ? unit.Title : "未知单位";
            }
        }
    }
}
