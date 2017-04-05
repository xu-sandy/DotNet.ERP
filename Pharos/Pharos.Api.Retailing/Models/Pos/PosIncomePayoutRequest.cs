using Pharos.Logic.ApiData.Pos.User;

namespace Pharos.Api.Retailing.Models.Pos
{
    /// <summary>
    /// 出入款参数
    /// </summary>
    public class PosIncomePayoutRequest : BaseApiParams
    {
        /// <summary>
        /// 员工工号
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 入款金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 类型（0：出款，1：入款）
        /// </summary>
        public PosIncomePayoutMode Type { get; set; }
    }
}