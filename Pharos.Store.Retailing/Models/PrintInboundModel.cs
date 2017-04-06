using Pharos.Logic;
using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharos.Store.Retailing.Models
{
    public class PrintInboundModel
    {
        /// <summary>
        /// 入库单信息
        /// </summary>
        public InboundGoods InboundGood { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier { get; set; }
        /// <summary>
        /// 入库门店
        /// </summary>
        public string StoreName { get; set; }
        /// <summary>
        /// 入库员
        /// </summary>
        public string CreateUserName { get; set; }

    }
}