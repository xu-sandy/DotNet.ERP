using Pharos.Logic.OMS.DAL;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.IDAL;
using Pharos.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.BLL
{
    /// <summary>
    /// 交易订单Service
    /// </summary>
    public class TradeOrderService
    {
        public TradeOrderService()
        {
            TradeOrderRepost = new BaseRepository<TradeOrder>();
        }
        /// <summary>
        /// 获取最大交易流水号
        /// </summary>
        IBaseRepository<TradeOrder> TradeOrderRepost { get; set; }
        public int GetMaxTradeNo()
        {
            var result = 0;
            var data = TradeOrderRepost.GetQuery().Max(o => o.TradeNo);
            if (data != null)
            {
                var orderNo = StringHelper.GetLastStr(data, 10);
                result = Convert.ToInt32(orderNo);
            }
            return result;
        }
        /// <summary>
        /// 获取最大商户支付订单号
        /// </summary>
        /// <returns></returns>
        public int GetMaxOutOrderNo()
        {
            var result = 0;
            var data = TradeOrderRepost.GetQuery().Max(o => o.OutTradeNo);
            if (data != null)
            {
                var orderNo = StringHelper.GetLastStr(data,10);
                result = Convert.ToInt32(orderNo);
            }
            return result;
        }
    }
}
