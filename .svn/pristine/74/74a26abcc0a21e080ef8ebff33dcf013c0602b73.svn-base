using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Utility.Helpers;
using System.Collections.Specialized;
using QCT.Pay.Common;
using System.Data.Entity.SqlServer;

namespace Pharos.Logic.OMS.BLL
{
    /// <summary>
    /// 交易数据Service
    /// </summary>
    public class TradeService
    {
        [Ninject.Inject]
        IBaseRepository<TradeOrder> TradeOrderRepst { get; set; }
        [Ninject.Inject]
        IBaseRepository<PayChannelManage> PChannelManageRepst { get; set; }

        public object GetTradeDataPaging(NameValueCollection reqParams, out int totalCount)
        {
            var queryParams = new
            {
                InputSelect1 = reqParams["InputSelect1"] == null ? "" : reqParams["InputSelect1"].ToString(),
                InputSelect2 = reqParams["InputSelect2"] == null ? "" : reqParams["InputSelect2"].ToString(),
                InputValue1 = reqParams["InputValue1"] == null ? "" : reqParams["InputValue1"].Trim(),
                InputValue2 = reqParams["InputValue2"] == null ? "" : reqParams["InputValue2"].Trim(),
                SourceType = reqParams["SourceType"] == null ? "" : reqParams["SourceType"].ToString(),
                ChannelNo = reqParams["ChannelNo"] == null ? "" : reqParams["ChannelNo"].ToString(),
                StartTradeDate = reqParams["StartTradeDate"] == null ? "" : reqParams["StartTradeDate"].ToString(),
                EndTradeDate = reqParams["EndTradeDate"] == null ? "" : reqParams["EndTradeDate"].ToString(),
            };
            #region query
            var query = TradeOrderRepst.GetQuery();
            if (!queryParams.InputValue1.IsNullOrEmpty())
            {
                switch (queryParams.InputSelect1)
                {
                    case "1"://商户订单号
                        query = query.Where(q => q.OutTradeNo == queryParams.InputValue1);
                        break;//交易流水号
                    case "2":
                        query = query.Where(q => q.TradeNo == queryParams.InputValue1);
                        break;
                    default:
                        break;
                }
            }
            if (!queryParams.InputValue2.IsNullOrEmpty())
            {
                var outVal = 0;
                switch (queryParams.InputSelect2)
                {
                    case "1"://商户号
                        int.TryParse(queryParams.InputValue2, out outVal);
                        query = query.Where(q => q.CID == outVal);
                        break;//门店号
                    case "2":
                        query = query.Where(q => q.SID == queryParams.InputValue2);
                        break;
                    case "3"://设备号
                        query = query.Where(q => q.DeviceId == queryParams.InputValue2);
                        break;
                    case "4"://第三方商户号
                        query = query.Where(q => q.MchId3 == queryParams.InputValue2);
                        break;
                    case "5"://第三方门店号
                        query = query.Where(q => q.StoreId3 == queryParams.InputValue2);
                        break;
                    default:
                        break;
                }
            }
            if (!queryParams.SourceType.IsNullOrEmpty())
            {//所属体系
                var value = (short)PaySourceType.CloudQctErp;
                short.TryParse(queryParams.SourceType, out value);
                query = query.Where(q => q.SourceType == value);
            }
            //if (!queryParams.PayChannel.IsNullOrEmpty())
            //{//支付渠道
            //    var value = (short)PayChannel.WX;
            //    short.TryParse(queryParams.PayChannel, out value);
            //    query = query.Where(q => q.PayChannel == value);
            //}
            if (!queryParams.ChannelNo.IsNullOrEmpty())
            {//支付通道
                var value = 0;
                int.TryParse(queryParams.ChannelNo, out value);
                query = query.Where(q => q.ChannelNo == value);
            }
            if (!queryParams.StartTradeDate.IsNullOrEmpty())
            {//订单时间开始时间
                var value = queryParams.StartTradeDate.Replace("-", "");
                var date = Convert.ToDateTime(queryParams.StartTradeDate);
                //query = query.Where(q => String.Compare(q.TradeDate, value, StringComparison.Ordinal) >= 0);
                query = query.Where(q => q.CreateDT >= date);
            }
            if (!queryParams.EndTradeDate.IsNullOrEmpty())
            {//订单时间开始时间
                var value = queryParams.EndTradeDate.Replace("-", "");
                var date = Convert.ToDateTime(queryParams.EndTradeDate).AddDays(1);
                //query = query.Where(q => String.Compare(q.TradeDate, value, StringComparison.Ordinal) <= 0);
                query = query.Where(q => q.CreateDT < date);
            }
            var query1 = (from s in query
                     join jpcm in PChannelManageRepst.GetQuery() on s.ChannelNo equals jpcm.ChannelNo into ipcm
                     from pcm in ipcm.DefaultIfEmpty()
                     select new TradeOrderExt()
                     {
                         Id = s.Id,
                         TradeNo = s.TradeNo,
                         OutTradeNo = s.OutTradeNo,
                         CID = s.CID,
                         SID = s.SID,
                         DeviceId = s.DeviceId,
                         MchId3 = s.MchId3,
                         StoreId3 = s.StoreId3,
                         DeviceId3 = s.DeviceId3,
                         FeeType = s.FeeType,
                         PayChannel = s.PayChannel,
                         ChannelNo = s.ChannelNo,
                         ChannelCode = pcm.ChannelCode,
                         SourceType = s.SourceType,
                         TradeType = s.TradeType,
                         TotalAmount = s.TotalAmount,
                         TradeDate = s.TradeDate,
                         TradeTime = s.TradeTime,
                         State = s.State,
                         CreateDT = s.CreateDT
                     });

            #endregion

            totalCount = query1.Count();
            var source = query1.ToPageList<TradeOrderExt>();
            var data = from s in source
                       select new TradeOrderExt()
                       {
                           Id = s.Id,
                           TradeNo = s.TradeNo,
                           OutTradeNo = s.OutTradeNo,
                           CID = s.CID,
                           SID = s.SID,
                           DeviceId = s.DeviceId,
                           MchId3 = s.MchId3,
                           StoreId3 = s.StoreId3,
                           DeviceId3 = s.DeviceId3,
                           FeeType = s.FeeType,
                           PayChannel = s.PayChannel,
                           ChannelNo = s.ChannelNo,
                           ChannelCode = s.ChannelCode,
                           SourceType = s.SourceType,
                           TradeType = s.TradeType,
                           TotalAmount = s.TotalAmount,
                           TradeDate = s.TradeDate,
                           TradeDateTime = PayTradeHelper.Convert2DateFormat(s.TradeDate, s.TradeTime, "yyyy-MM-dd HH:mm:ss"),
                           TradeTime = s.TradeTime,
                           State = s.State,
                           CreateDT = s.CreateDT
                       };
            return data;
        }
    }
}
