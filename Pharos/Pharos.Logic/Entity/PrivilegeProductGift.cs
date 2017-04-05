using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 设置赠品
    /// </summary>
    public class PrivilegeProductGift
    {
        public int Id { get; set; }
        /// <summary>
        /// 关联主表
        /// </summary>
        public int RegionValId { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }
        /// <summary>
        /// 赠品数量
        /// </summary>
        public short GiftNumber { get; set; }
    }
}
