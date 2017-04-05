using Pharos.Utility;
using System;

namespace Pharos.Logic.BLL.DataSynchronism.Dtos
{
    [Excel("POS出入款信息")]

    /// <summary>
    /// POS出入款信息
    /// </summary>
    public class PosIncomePayoutForLocal
    {
        /// <summary>
        /// 门店ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [Excel("门店ID", 1)]
        [ExcelField(@"^[0-9]{1,3}$###门店ID格式不正确或长度超过3位或为空")]
        public string StoreId { get; set; }


        /// <summary>
        /// POS机号
        /// [长度：20]
        /// [不允许为空]
        /// </summary>
        [Excel("POS机号", 2)]
        [ExcelField(@"^[\s,\S]{1,20}$###POS机号长度应在1-20位或为空")]
        public string MachineSN { get; set; }

        /// <summary>
        /// 收银员UID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [Excel("收银员", 3)]
        [ExcelField(@"^[\s,\S]{1,40}$###收银员长度应在1-40位或为空")]
        public string CreateUID { get; set; }

        /// <summary>
        /// 类型（0:出款、1:入款） 
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        [Excel("类型", 4)]
        [ExcelField(@"^[0-9]$###类型格式不正确或长度超过1位或为空")]
        public short Type { get; set; }

        /// <summary>
        /// 金额
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Excel("金额", 5)]
        [ExcelField(@"^[\d+(\.\d+)?]{1,19}$###金额格式不正确或长度超过19位或为空")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        [Excel("时间", 6)]
        [ExcelField(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$###时间格式不正确(yyyy-MM-dd HH:mm:ss)或为空")]
        public DateTime CreateDT { get; set; }
    }
}
