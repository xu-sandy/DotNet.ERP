using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public class PayConfiguration
    {
        public int Id { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 第三方支付平台提供的支付安全密钥
        /// </summary>
        public string SecurityKey { get; set; }

        /// <summary>
        /// 第三方支付商户号
        /// </summary>
        public string PaymentMerchantNumber { get; set; }

        /// <summary>
        /// 支付类型
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 向客户显示的支付描述（如沃尔玛购物）
        /// </summary>
        public string Description { get; set; }
    }
}
