﻿using Pharos.POS.Retailing.Models.PosModels;

namespace Pharos.POS.Retailing.Models.ApiParams
{
    public class SaleParams : BaseApiParams
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Number { get; set; }
        /// <summary>
        /// 售价
        /// </summary>
        public decimal SalePrice { get; set; }
        /// <summary>
        /// 销售状态
        /// </summary>
        public SaleStatus Status { get; set; }
        public string RecordId { get; set; }
        /// <summary>
        /// 是否可编辑
        /// </summary>
        public bool HasEditPrice { get; set; }
    }
}
