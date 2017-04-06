using Pharos.Sys.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public abstract class BaseOrder : BaseEntity
    {
        /// <summary>
        /// 记录 ID
        /// [主键：√]
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [OperationLog("ID", false)]
        public int Id { get; set; }
        /// <summary>
        /// 采购单ID(订货单ID)
        /// [长度：40]
        /// [不允许为空] 
        /// </summary>
        [OperationLog("采购单", false)]
        public string IndentOrderId { get; set; }
        /// <summary>
        /// 订货门店 ID
        /// [长度：3]
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 供货单位 ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [OperationLog("供货单位", false)]
        public string SupplierID { get; set; }
        /// <summary>
        /// 收货人ID
        /// [长度：40]
        /// </summary>
        [OperationLog("收货人", false)]
        public string RecipientsUID { get; set; }

        /// <summary>
        /// 订单总额
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// </summary>
        [OperationLog("订单总额", false)]
        public decimal OrderTotal { get; set; }

        /// <summary>
        /// 收货地址
        /// [长度：100]
        /// </summary>
        [OperationLog("收货地址", false)]
        public string ShippingAddress { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [OperationLog("联系电话", false)]
        public string Phone { get; set; }
        /// <summary>
        /// 交货日期
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        [OperationLog("交货日期", false)]
        public string DeliveryDate { get; set; }
        /// <summary>
        /// 配送开始日期
        /// [长度：10]
        /// </summary>
        [OperationLog("配送开始日期", false)]
        public string PeiSongStartDate { get; set; }
        /// <summary>
        /// 配送完成日期
        /// [长度：10]
        /// </summary>
        [OperationLog("配送完成日期", false)]
        public string PeiSongEndDate { get; set; }
        /// <summary>
        /// 创建时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// </summary>
        [Newtonsoft.Json.JsonConverter(typeof(Pharos.Utility.JsonShortDate))]
        [OperationLog("创建时间", false)]
        public DateTime CreateDT { get; set; }
        /// <summary>
        /// 已收货日期
        /// </summary>
        [OperationLog("已收货日期", false)]
        public DateTime? ReceivedDT { get; set; }
        /// <summary>
        /// 创建人 UID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [OperationLog("创建人", false)]
        public string CreateUID { get; set; }
        /// <summary>
        /// 审批时间
        /// </summary>
        [OperationLog("审批时间", false)]
        public DateTime? ApproveDT { get; set; }
        /// <summary>
        /// 状态（ -1:未提交、0:未审核、 1:已审核（ 未配送）、 2:配送中、 3:已中止、4:已配送、 5:已收货）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [OperationLog("状态", "-1:未提交", "0:未审核", "1:已审核", "2:配送中", "3:已中止", "4:已配送", "5:已收货")]
        public short State { get; set; }
    }
}
