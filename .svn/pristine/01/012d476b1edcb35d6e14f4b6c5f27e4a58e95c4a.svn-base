using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public class VwOrder : BaseOrder
    {
        public string StoreTitle { get; set; }
        /// <summary>
        /// 订货人
        /// </summary>
        public string OrderTitle { get; set; }
        public string SupplierTitle { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public string RecipientsTitle { get; set; }
        /// <summary>
        /// 下单人
        /// </summary>
        public string CreateTitle { get; set; }
        /// <summary>
        /// 订货量
        /// </summary>
        public decimal IndentNums { get; set; }
        /// <summary>
        /// 出售量
        /// </summary>
        public decimal DeliveryNums { get; set; }
        /// <summary>
        /// 收货量
        /// </summary>
        public decimal AcceptNums { get; set; }
        /// <summary>
        /// 配送批次
        /// </summary>
        public string DistributionBatch { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// 订单数/赠品数
        /// </summary>
        public string OrderGiftnum { get; set; }

        public string StateTitle
        {
            get { return Enum.GetName(typeof(OrderState),State);}
        }
       
        public string gId { get { return Guid.NewGuid().ToString(); } }
    }
}
