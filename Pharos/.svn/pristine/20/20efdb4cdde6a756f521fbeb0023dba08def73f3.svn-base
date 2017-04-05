#if(POS!=true)
using Pharos.Sys.Extensions;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 产品变价
    /// </summary>
    public class ProductChangePrice:BaseEntity
    {
#if(POS!=true)
        [OperationLog("ID", false)]
#endif
        public string Id { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
#if(POS!=true)
        [OperationLog("供应商", false)]
#endif
        public string SupplierId { get; set; }
        /// <summary>
        /// 调价门店，多个以逗号隔开
        /// </summary>
#if(POS!=true)
        [OperationLog("调价门店", false)]
#endif
        public string StoreId { get; set; }
        /// <summary>
        /// 录入人
        /// </summary>
#if(POS!=true)
        [OperationLog("录入人", false)]
#endif
        public string CreateUID { get; set; }
        /// <summary>
        /// 审批人
        /// </summary>
#if(POS!=true)
        [OperationLog("顺序", false)]
#endif
        public string AuditorUID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
#if(POS!=true)
        [OperationLog("创建时间", false)]
#endif
        public DateTime CreateDT { get; set; }
        /// <summary>
        /// 状态 0-未审批1-已审批2-已失效
        /// </summary>
#if(POS!=true)
        [OperationLog("状态", "0:未审批", "1:已审批", "2:已失效")]
#endif
        public short State { get; set; }
        /// <summary>
        /// 审批时间
        /// </summary>
#if(POS!=true)
        [OperationLog("审批时间", false)]
#endif
        public DateTime? AuditorDT { get; set; }
#if(POS!=true)
        [OperationLog("变更列表", false)]
#endif
        public List<ProductChangePriceList> ChangePriceList { get; set; }
#if(POS!=true)
        [Pharos.Utility.Exclude]
#endif
        public byte[] SyncItemVersion { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
#if(POS!=true)
        [Pharos.Utility.Exclude]
#endif
        public Guid SyncItemId { get; set; }
    }
    /// <summary>
    /// 变价子表
    /// </summary>
    public class ProductChangePriceList
    {
#if(POS!=true)
        [OperationLog("Id", false)]
#endif
        public int Id { get; set; }
#if(POS!=true)
        [OperationLog("更改id", false)]
#endif
        public string ChangePriceId { get; set; }
#if(POS!=true)
        [OperationLog("条码", false)]
#endif
        public string Barcode { get; set; }
        /// <summary>
        /// 原进价
        /// </summary>
#if(POS!=true)
        [OperationLog("原进价", false)]
#endif
        public decimal OldBuyPrice { get; set; }
        /// <summary>
        /// 现进价
        /// </summary>
#if(POS!=true)
        [OperationLog("现进价", false)]
#endif
        public decimal CurBuyPrice { get; set; }
        /// <summary>
        /// 原售价
        /// </summary>
#if(POS!=true)
        [OperationLog("原售价", false)]
#endif
        public decimal OldSysPrice { get; set; }
        /// <summary>
        /// 现售价
        /// </summary>
#if(POS!=true)
        [OperationLog("现售价", false)]
#endif
        public decimal CurSysPrice { get; set; }
        /// <summary>
        ///原毛利率（%）
        /// </summary>
#if(POS!=true)
        [OperationLog("原毛利率", false)]
#endif
        public decimal OldGrossprofitRate { get; set; }
        /// <summary>
        /// 现毛利率（%）
        /// </summary>
#if(POS!=true)
        [OperationLog("现毛利率", false)]
#endif
        public decimal CurGrossprofitRate { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
#if(POS!=true)
        [OperationLog("开始时间", false)]
#endif
        public string StartDate { get; set; }
        /// <summary>
        /// 结束时间 NULL-不限
        /// </summary>
#if(POS!=true)
        [OperationLog("结束时间", false)]
#endif
        public string EndDate { get; set; }
#if(POS!=true)
        [OperationLog("备注", false)]
#endif
        public string Memo { get; set; }
        /// <summary>
        /// 状态（ 0:无效、 1:有效）
        /// </summary>
#if(POS!=true)
        [OperationLog("状态", "0:无效", "1:有效")]
#endif
        public short State { get; set; }

#if(POS!=true)
        [Pharos.Utility.Exclude]
#endif
        public byte[] SyncItemVersion { get; set; }
#if(POS!=true)
        [Pharos.Utility.Exclude]
#endif
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Guid SyncItemId { get; set; }
    }
}
