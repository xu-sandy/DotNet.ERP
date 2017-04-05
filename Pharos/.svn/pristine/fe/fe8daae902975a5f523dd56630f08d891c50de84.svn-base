using Pharos.Utility;
using System;

namespace Pharos.Logic.BLL.DataSynchronism.Dtos
{
    [Excel("消费支付方式")]
    /// <summary>
    /// 消费支付方式
    /// </summary>
    public class ConsumptionPaymentForLocal
    {

        
        /// <summary>
        /// 流水号
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        [Excel("流水号", 1)]
        [ExcelField(@"^[\s,\S]{1,50}$###POS机号长度应在1-50位或为空")]
        public string PaySN { get; set; }
        

        /// <summary>
        /// 支付方式 ID
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        [Excel("支付方式 ID", 2)]
        [ExcelField(@"^[0-9]{1,10}$###支付方式ID格式不正确或长度超过10位或为空")]
        public int ApiCode { get; set; }

        /// <summary>
        /// 交易单号（由支付宝或微信返回）
        /// [长度：50]
        /// </summary>
        [Excel("交易单号", 3)]
        [ExcelField(@"^[\s,\S]{0,50}$###交易单号长度应在0-50位或为空")]
        public string ApiOrderSN { get; set; }

        /// <summary>
        /// 金额
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// </summary>
        [Excel("金额", 4)]
        [ExcelField(@"^[\d+(\.\d+)?]{1,19}$###金额格式不正确或长度超过19位或为空")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 备注
        /// [长度：200]
        /// </summary>
        [Excel("备注", 5)]
        [ExcelField(@"^[\s,\S]{0,200}$###交易单号长度应在0-200位")]
        public string Memo { get; set; }

        /// <summary>
        /// 支付状态（0：未支付、1：已支付）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Excel("支付状态", 6)]
        [ExcelField(@"^[0-9]$###支付状态格式不正确或长度超过1位或为空")]
        public short State { get; set; }

    }
}
