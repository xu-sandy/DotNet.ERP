using Pharos.Sys.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public class ProductTradePrice
    {
        public int CompanyId { get; set; }
        [OperationLog("Id", false)]
        public string Id { get; set; }
        /// <summary>
        /// 批发商，多个以逗号隔开
        /// </summary>
        [OperationLog("批发商", false)]
        public string Wholesaler { get; set; }
        
        /// <summary>
        /// 状态 0-未审批1-已审批2-已失效
        /// </summary>
        [OperationLog("状态", "0:未审批", "1:已审批", "2:已失效")]
        public short State { get; set; }
        /// <summary>
        /// 审批时间
        /// </summary>
        [OperationLog("审批时间", false)]
        public DateTime? AuditorDT { get; set; }
        /// <summary>
        /// 录入人
        /// </summary>
        [OperationLog("录入人", false)]
        public string CreateUID { get; set; }
        /// <summary>
        /// 审批人
        /// </summary>
        [OperationLog("审批人", false)]
        public string AuditorUID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [OperationLog("创建时间", false)]
        public DateTime CreateDT { get; set; }

        [OperationLog("列表", false)]
        public List<ProductTradePriceList> TradePriceList { get; set; }
    }
    public class ProductTradePriceList
    {
        [OperationLog("Id", false)]
        public int Id { get; set; }
        public string TradePriceId { get; set; }
        [OperationLog("条码", false)]
        public string Barcode { get; set; }
        /// <summary>
        /// 现进价
        /// </summary>
        [OperationLog("现进价", false)]
        public decimal BuyPrice { get; set; }
        /// <summary>
        /// 现售价
        /// </summary>
        [OperationLog("现售价", false)]
        public decimal SysPrice { get; set; }
        /// <summary>
        /// 批发价
        /// </summary>
        [OperationLog("批发价", false)]
        public decimal TradePrice { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [Newtonsoft.Json.JsonConverter(typeof(Pharos.Utility.JsonShortDate))]
        [OperationLog("开始时间", false)]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 结束时间 NULL-不限
        /// </summary>
        [Newtonsoft.Json.JsonConverter(typeof(Pharos.Utility.JsonShortDate))]
        [OperationLog("结束时间", false)]
        public DateTime? EndDate { get; set; }
        [OperationLog("备注", false)]
        public string Memo { get; set; }
    }
}
