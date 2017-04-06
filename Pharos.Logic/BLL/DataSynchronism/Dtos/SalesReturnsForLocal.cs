using Pharos.Utility;
using System;

namespace Pharos.Logic.BLL.DataSynchronism.Dtos
{
    [Excel("售后退换信息")]

    /// <summary>
    /// 售后退换信息
    /// </summary>
    public class SalesReturnsForLocal
    {

        /// <summary>
        /// 退换方式（0:退货、1:换货）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        [Excel("退换方式", 1)]
        [ExcelField(@"^[0-9]$###退换方式不正确或长度超过1位或为空")]
        public short ReturnType { get; set; }
        

        /// <summary>
        /// 退换货ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [ExcelField(@"^[\s,\S]{1,40}$###退换货ID长度应在1-40位或为空")]
        [Excel("退换货ID", 2)]
        public string ReturnId { get; set; }

        /// <summary>
        /// 门店ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [Excel("门店ID", 3)]
        [ExcelField(@"^[0-9]{1,3}$###门店ID格式不正确或长度超过3位或为空")]
        public string StoreId { get; set; }

        /// <summary>
        /// 新单据号
        ///  [长度：50]
        /// </summary>
        [Excel("新单据号", 4)]
        [ExcelField(@"^[\s,\S]{1,40}$###新单据号长度应在1-50位或为空")]
        public string NewPaySN { get; set; }

        /// <summary>
        /// 退换理由ID（来自数据字典）
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        [Excel("退换理由ID", 5)]
        [ExcelField(@"^[0-9]{1,10}$###退换理由ID格式不正确或长度超过10位或为空")]
        public int ReasonId { get; set; }

        /// <summary>
        /// 退换差价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// </summary>
        [Excel("退换差价", 6)]
        [ExcelField(@"^[\d+(\.\d+)?]{1,19}$###退换差价格式不正确或长度超过19位或为空")]
        public decimal ReturnPrice { get; set; }

        /// <summary>
        /// 退换时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        [Excel("退换时间", 7)]
        [ExcelField(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$###时间格式不正确(yyyy-MM-dd HH:mm:ss)或为空")]
        public DateTime CreateDT { get; set; }

        /// <summary>
        /// 经办人UID 
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [Excel("经办人UID", 8)]
        [ExcelField(@"^[\s,\S]{1,40}$###收银员长度应在1-40位或为空")]
        public string CreateUID { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Excel("状态", 9)]
        [ExcelField(@"^[0-9]$###状态格式不正确或长度超过1位或为空")]
        public short State { get; set; }
        public string MachineSN { get; set; }

    }
}
