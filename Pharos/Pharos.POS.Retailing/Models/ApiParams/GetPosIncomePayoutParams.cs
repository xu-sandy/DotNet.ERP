﻿
namespace Pharos.POS.Retailing.Models.ApiParams
{
    /// <summary>
    /// 获取现金出入款接口提交参数
    /// </summary>
    public class GetPosIncomePayoutParams : BaseApiParams
    {
        /// <summary>
        /// 收银员工号
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// 收银员密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 出入款金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public int Type { get; set; }
    }
}
