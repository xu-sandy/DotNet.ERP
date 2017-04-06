using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Entity.View
{
    /// <summary>
    /// 采购意向清单
    /// </summary>
    public class ViewOrderList
    {
        /// <summary>
        /// 记录 ID
        /// [主键：√]
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// CID
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int CID { get; set; }

        /// <summary>
        /// 设备分类（来自字典表）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short DeviceId { get; set; }

        /// <summary>
        /// 设备名称
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 预购数量
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short OrderNum { get; set; }

        /// <summary>
        /// 创建时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        public DateTime CreateDT { get; set; }

        /// <summary>
        /// 操作人 UID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CreateUID { get; set; }

        /// <summary>
        /// 单位（来自字典）
        /// [长度：10]
        /// </summary>
        public int UnitID { get; set; }

        /// <summary>
        /// 备注
        /// [长度：200]
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string pName { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string uName { get; set; }
    }
}
