﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.POS.Retailing.Models.Printer
{
    public class TicketModel
    {
        #region 发票格式参数
        /// <summary>
        /// 发票宽度，按字符数计算，根据打印机型号有所区别(通常在30-70之间)
        /// </summary>
        public int TicketWidth { get; set; }

        public int OrderType { get; set; }
        #endregion

        #region 发票内容
        /// <summary>
        /// 店名
        /// </summary>
        public string StoreName { get; set; }
        /// <summary>
        /// 机号
        /// </summary>
        public string DeviceNumber { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public string SN { get; set; }
        /// <summary>
        /// 收银员
        /// </summary>
        public string Cashier { get; set; }
        /// <summary>
        /// 购买商品列表
        /// </summary>
        public List<ProductModel> ProductList { get; set; }
        /// <summary>
        /// 合计件数
        /// </summary>
        public int CountNum { get; set; }
        /// <summary>
        /// 合计金额
        /// </summary>
        public string TotalPrice { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayType { get; set; }
        /// <summary>
        /// 发票金额
        /// </summary>
        public string Receivable { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        // public int Integral { get; set; }
        /// <summary>
        /// 现金找零
        /// </summary>
        public decimal Change { get; set; }
        /// <summary>
        /// 称重
        /// </summary>
        public string Weigh { get; set; }
        /// <summary>
        /// 底部信息
        /// </summary>
        public List<string> FootItemList { get; set; }

        /// <summary>
        /// 折让
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// 导购员
        /// </summary>
        public string SaleMan { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal Preferential { get; set; }
        /// <summary>
        /// 订单时间
        /// </summary>
        public DateTime CreateDT { get; set; }
        /// <summary>
        /// 订单支付方式
        /// </summary>
        public string ApiCodes { get; set; }
        /// <summary>
        /// 会员卡支付时剩余的余额
        /// </summary>
        public List<Dictionary<string, string>> CardAndBalances { get; set; }
        /// <summary>
        /// 充值小票数据
        /// </summary>
        public RechargeModel rechargeModel { get; set; }
        #endregion
    }
}
