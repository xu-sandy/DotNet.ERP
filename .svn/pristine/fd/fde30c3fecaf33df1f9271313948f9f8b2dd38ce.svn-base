using Pharos.Utility;
using System;

namespace Pharos.Logic.BLL.DataSynchronism.Dtos
{
    [Excel("会员积分明细")]

    /// <summary>
    /// 会员积分明细
    /// </summary>
    public class MemberIntegralForLocal
    {

        
        /// <summary>
        /// 流水号
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        [Excel("流水号", 1)]
        [ExcelField(@"^[\s,\S]{1,50}$###用户长度应在1-50位或为空")]
        public string PaySN { get; set; }

        
        /// <summary>
        /// 会员卡号
        /// [长度：100]
        /// [不允许为空]
        /// </summary>
        [Excel("会员卡号", 2)]
        [ExcelField(@"^[\s,\S]{1,100}$###用户长度应在1-100位或为空")]
        public string MemberId { get; set; }

        /// <summary>
        /// 消费金额
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Excel("消费金额", 3)]
        [ExcelField(@"^[\d+(\.\d+)?]{1,19}$###消费金额格式不正确或长度超过19位或为空")]
        public decimal ActualPrice { get; set; }

        /// <summary>
        /// 兑换积分
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Excel("兑换积分", 4)]
        [ExcelField(@"^[0-9]{1,10}$###兑换积分格式不正确或长度超过10位或为空")]
        public int Integral { get; set; }

        /// <summary>
        /// 消费时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        [Excel("消费时间", 5)]
        [ExcelField(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$###消费时间格式不正确(yyyy-MM-dd HH:mm:ss)或为空")]
        public DateTime CreateDT { get; set; }
    }
}
