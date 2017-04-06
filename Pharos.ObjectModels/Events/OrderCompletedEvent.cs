﻿//using Pharos.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.ObjectModels.Events
{
    public class OrderCompletedEvent : IIntegralEvent
    {
        public Guid ID
        {
            get { return Guid.NewGuid(); }
        }

        public DateTime TimeStamp
        {
            get { return DateTime.UtcNow; }
        }
        public string MemberId { get; set; }
        public int CompanyId { get; set; }
        public string StoreId { get; set; }
        public string MachineSn { get; set; }
        public string SourceRecordId { get; set; }
        public int OrderProductCount { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal OrderReceiveAmount { get; set; }
        public Dictionary<int, decimal> Pays { get; set; }

        public IEnumerable<OrderDetail> OrderDetails { get; set; }

        public string OperatorUid { get; set; }

    }
    public class OrderDetail
    {
        public decimal Total { get; set; }

        public decimal AveragePrice { get; set; }

        public string ProductCode { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 商品条码
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        public string Barcode { get; set; }
        public string ScanBarcode { get; set; }

        public string Title { get; set; }

        /// <summary>
        /// 购买数量
        /// [不允许为空]
        /// </summary>
        public decimal PurchaseNumber { get; set; }


        /// <summary>
        /// 系统进价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal BuyPrice { get; set; }


        /// <summary>
        /// 系统售价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal SysPrice { get; set; }


        /// <summary>
        /// 交易价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal ActualPrice { get; set; }

        public int SalesClassifyId { get; set; }
    }
}