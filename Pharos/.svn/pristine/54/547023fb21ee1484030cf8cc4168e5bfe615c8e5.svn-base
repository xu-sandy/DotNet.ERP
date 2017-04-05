using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public class VwSaleDetail
    {
        /// <summary>
        /// SaleDetail主键
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// 流水号 
        /// </summary>
        public string PaySN { get; set; }


        /// <summary>
        /// 商品条码
        /// </summary>
        public string Barcode { get; set; }


        /// <summary>
        /// 商品名称
        /// </summary>
        public string Title { get; set; }
        

        /// <summary>
        /// 购买数量
        /// </summary>
        public decimal PurchaseNumber { get; set; }


        /// <summary>
        /// 系统进价
        /// </summary>
        public decimal BuyPrice { get; set; }


        /// <summary>
        /// 系统售价
        /// </summary>
        public decimal SysPrice { get; set; }


        /// <summary>
        /// 交易价
        /// </summary>
        public decimal ActualPrice { get; set; }


        /// <summary>
        /// 销售分类ID（来自数据字典） 
        /// </summary>
        public int SalesClassifyId { get; set; }

        /// <summary>
        /// 发生的退换次数
        /// </summary>
        public int HasReturned { get; set; }

        /// <summary>
        /// 退换的商品总数
        /// </summary>
        public decimal? ReturnedNumber { get; set; }

        /// <summary>
        /// 门店Id
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// 退换ID
        /// </summary>
        public string ReturnId { get; set; }

        /// <summary>
        /// 账单类型(0：正常销售；1：换货) 默认值：0
        /// </summary>
        public short Type { get; set; }

        /// <summary>
        /// 状态（默认：0，0：正常，1：已退出整单）
        /// </summary>
        public int State { get; set; }
    }
}
