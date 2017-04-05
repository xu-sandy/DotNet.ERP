using Newtonsoft.Json;
using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Logic.LocalEntity;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.LocalEntity
{
    /// <summary>
    /// 促销活动量记录
    /// </summary>
    public class SalesRecord : BaseEntity, ICanUpdateEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// 促销Id
        /// </summary>
        public string CommodityId { get; set; }
        /// <summary>
        /// 商店Id
        /// </summary>
        [ExcelField(@"^[0-9]{1,2}$###门店ID长度应在1-2位且为数字")]
        public string StoreId { get; set; }
        /// <summary>
        /// 销售数
        /// </summary>
        public decimal Number { get; set; }
        /// <summary>
        /// 日期 （每日限购不能为空）
        /// </summary>
        public string CreateDT { get; set; }
        [JsonIgnore]

        public bool HasUpdate { get; set; }
    }
}
