﻿using Pharos.Logic.OMS.DAL;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QCT.Pay.Common.Models;
using QCT.Pay.Common;
using Pharos.Logic.OMS.Models;

namespace Pharos.Logic.OMS.BLL
{
    /// <summary>
    /// 提供给ERP调用的支付相关接口
    /// </summary>
    public class PayForERPService
    {
        public PayForERPService()
        {
            PayApiRepost = new BaseRepository<PayApi>();
            TPaySecretKeyRepost = new BaseRepository<TradersPaySecretKey>();
            TStoreRepost = new BaseRepository<TradersStore>();
        }
        /// <summary>
        /// 支付接口Service类
        /// </summary>
        PayService PaySvc { get; set; }
        /// <summary>
        /// 交易支付接口
        /// </summary>
        IBaseRepository<PayApi> PayApiRepost { get; set; }
        /// <summary>
        /// 商家支付主密钥
        /// </summary>
        IBaseRepository<TradersPaySecretKey> TPaySecretKeyRepost { get; set; }
        /// <summary>
        /// 商家门店
        /// </summary>
        IBaseRepository<TradersStore> TStoreRepost { get; set; }
        /// <summary>
        /// 根据CID获取商户的支付类型列表
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public QctPayReturn GetMerchPayModes(int cid)
        {
            var payLicenseObj = PaySvc.GetPayLicense(cid, (short)TraderPayLicenseState.Audited);
            if (payLicenseObj == null)
                return QctPayReturn.Fail("请先申请支付许可");
            var merchObj = PaySvc.GetMerchByCID(cid);
            if (merchObj == null)
                return QctPayReturn.Fail("请先申请开通商户，若您已经申请，请等待审核");

            var query = (from p in PayApiRepost.GetQuery()
                         join jtpsk in TPaySecretKeyRepost.GetQuery() on p.ChannelNo equals jtpsk.ChannelNo into itpsk
                         from tpsk in itpsk.DefaultIfEmpty()
                         where tpsk.CID == cid
                         && p.State == (short)PayApiState.HasReleased && p.OptType == (short)PayOperateType.Receipt
                         select new MerchPayModeItem()
                         {
                             Title = p.Title,
                             ChannelNo = p.ChannelNo,
                             PayMode = p.TradeMode,
                             State = DataItemState.Enabled
                         });
            var objs = query.ToList();
            var data = new MerchPayMode()
            {
                CID = cid,
                Items = objs
            };
            return QctPayReturn.Success(data: objs);
        }
        /// <summary>
        /// 根据商户信息获得门店支付权限，若只传商户ID，则返回全部门店的状态信息，若传门店ID则只返回指定门店的支付权限信息
        /// </summary>
        /// <param name="cid">商户号</param>
        /// <param name="storeId">门店号(可选)</param>
        /// <returns></returns>
        public QctPayReturn GetMerchStorePayInfos(int cid, string storeId="")
        {
            var merchObj = PaySvc.GetMerchByCID(cid);
            if (merchObj == null)
                return QctPayReturn.Fail("请先申请开通商户，若您已经申请，请等待审核");

            var query = TStoreRepost.GetQuery().Where(o => o.CID == cid);
            if (string.IsNullOrWhiteSpace(storeId))
            {
                query = query.Where(o => o.State == (short)TraderStoreState.Enabled);
            }
            else
            {
                query = query.Where(o => o.StoreNum == storeId);
            }
            var queryStore = (from s in query
                              select new MerchStorePayItem
                              {
                                  StoreId = s.StoreNum,
                                  State = s.State == (short)TraderStoreState.Enabled ? DataItemState.Enabled : DataItemState.Disabled
                              }).ToList();
            var data = new MerchStorePayModel()
            {
                CID = cid,
                Items = queryStore
            };
            return QctPayReturn.Success(data: data);
        }
    }
}
