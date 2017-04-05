using Pharos.Utility;
using System;

namespace Pharos.Logic.BLL.DataSynchronism.Dtos
{
    [Excel("销售单信息")]

    /// <summary>
    /// 销售单信息
    /// </summary>
    public class SaleOrdersForLocal
    {
        
        /// <summary>
        /// 流水号（全局唯一）
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        [Excel("流水号", 1)]
        [ExcelField(@"^[\s,\S]{1,50}$###POS机号长度应在1-50位或为空")]
        public string PaySN { get; set; }
        /// <summary>
        /// 门店ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [Excel("门店ID", 2)]
        [ExcelField(@"^[0-9]{1,3}$###门店ID格式不正确或长度超过3位或为空")]
        public string StoreId { get; set; }

        /// <summary>
        /// POS机号
        /// [长度：20]
        /// [不允许为空]
        /// </summary>
        [Excel("POS机号", 3)]
        [ExcelField(@"^[\s,\S]{1,20}$###POS机号长度应在1-20位或为空")]
        public string MachineSN { get; set; }

        /// <summary>
        /// 金额合计（优惠前)
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Excel("金额合计", 4)]
        [ExcelField(@"^[\d+(\.\d+)?]{1,19}$###金额合计格式不正确或长度超过19位或为空")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 优惠合计
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// </summary>
        [ExcelField(@"^[\d+(\.\d+)?]{1,19}$###优惠合计格式不正确或长度超过19位或为空")]
        [Excel("优惠合计", 5)]
        public decimal PreferentialPrice { get; set; }

        /// <summary>
        /// 支付方式ID（多个ID以,号间隔）
        /// [长度：100]
        /// [不允许为空]
        /// </summary>
        [Excel("支付方式ID", 6)]
        [ExcelField(@"^[\s,\S]{1,100}$###支付方式长度应在1-100位或为空")]
        public string ApiCode { get; set; }

        /// <summary>
        /// 交易时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        [Excel("交易时间", 7)]
        [ExcelField(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$###交易时间格式不正确(yyyy-MM-dd HH:mm:ss)或为空")]
        public DateTime CreateDT { get; set; }

        /// <summary>
        /// 收银员UID 
        /// [长度：40]
        /// </summary>
        [Excel("收银员UID", 8)]
        [ExcelField(@"^[\s,\S]{1,100}$###收银员长度应在1-40位")]
        public string CreateUID { get; set; }

        /// <summary>
        /// 导购员UID
        /// [长度：40]
        /// </summary>
        [Excel("导购员UID", 9)]
        [ExcelField(@"^[\s,\S]{0,100}$###导购员长度应在1-40位")]
        public string Salesman { get; set; }
        /// <summary>
        /// 备注
        /// [长度：200]
        /// </summary>
        [Excel("备注", 10)]
        [ExcelField(@"^[\s,\S]{0,200}$###备注长度应在1-200位")]
        public string Memo { get; set; }
        /// <summary>
        /// 账单类型(0：正常销售；1：换货)
        /// </summary>
        [Excel("账单类型", 11)]
        [ExcelField(@"^[0-9]$###类型格式不正确或长度超过1位或为空")]
        public short Type { get; set; }
        public string MemberId { get; set; }

        public int State { get; set; }

        public string ReturnId { get; set; }


    }
}
