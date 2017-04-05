using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public class WeighingBatch : BaseEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// 1-导出,2-传秤
        /// </summary>
        public short Communication { get; set; }
        /// <summary>
        /// 批次
        /// 联机:门店-称号-年月日
        /// 导出:门店-(F+序号(01-99))-年月日
        /// </summary>
        public string BatchNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 已选商品编码，以逗号隔开
        /// </summary>
        public string Details { get; set; }
        public DateTime CreateDT { get; set; }
        public string CreateUID { get; set; }
    }
}
