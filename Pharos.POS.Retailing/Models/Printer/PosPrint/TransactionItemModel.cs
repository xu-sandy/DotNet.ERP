using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.POS.Retailing.Models.Printer
{
    public class TransactionItemModel
    {


        public TransactionItemModel() { }
        public TransactionItemModel(string transactionName, decimal totalAmount)
        {
            this.TransactionName = transactionName;
            this.StrokeCount = null;
            this.TotalAmount = totalAmount;
        }
        public TransactionItemModel(string transactionName, int strokeCount, decimal totalAmount)
        {
            this.TransactionName = transactionName;
            this.StrokeCount = strokeCount;
            this.TotalAmount = totalAmount;
        }
        public TransactionItemModel(string transactionName, int strokeCount, decimal totalAmount, Dictionary<string, decimal> childItems)
        {
            this.TransactionName = transactionName;
            this.StrokeCount = strokeCount;
            this.TotalAmount = totalAmount;
            this.ChildItems = childItems;
        }
        /// <summary>
        /// 交易名称
        /// </summary>
        public string TransactionName { get; set; }
        /// <summary>
        /// 笔数
        /// </summary>
        public int? StrokeCount { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 子项
        /// </summary>
        public Dictionary<string, decimal> ChildItems { get; set; }

    }
}
