using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public class StorePaymentAuthorization
    {
        public int Id { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 门店Id
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 支付平台开通门店映射
        /// </summary>
        public string MapPaymentStoreId { get; set; }
        /// <summary>
        /// 支付类型
        /// </summary>
        public int PayType { get; set; }
        /// <summary>
        /// 是否已经授权
        /// </summary>
        public int State { get; set; }

    }
}
