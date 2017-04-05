﻿using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.IDAL;
using Pharos.Logic.OMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Utility.Helpers;
using Pharos.Logic.OMS.DAL;
using System.ComponentModel;
using QCT.Pay.Common;
using QCT.Pay.Common.Models;
using QCT.Pay.Common.Helpers;

namespace Pharos.Logic.OMS.BLL
{
    /// <summary>
    /// 支付接口Service类
    /// </summary>
    public class PayService
    {
        /// <summary>
        /// 支付订单记录仓储
        /// </summary>
        private readonly IBaseRepository<TradeOrder> _tradeOrderRepost;
        /// <summary>
        /// 支付结果通知仓储
        /// </summary>
        private readonly IBaseRepository<TradeResult> _tradeResultRepost;
        /// <summary>
        /// 商户信息
        /// </summary>
        private readonly IBaseRepository<Traders> _tradeRepost;
        /// <summary>
        /// 商家支付许可
        /// </summary>
        private readonly IBaseRepository<PayLicense> _tPayLicenseRepost;
        /// <summary>
        /// 商家结算账户
        /// </summary>
        private readonly IBaseRepository<BankAccount> _tBankAccountRepost;
        /// <summary>
        /// 商家支付主密钥
        /// </summary>
        private readonly IBaseRepository<TradersPaySecretKey> _tPaySecretKeyRepost;
        /// <summary>
        /// 商家支付通道
        /// </summary>
        private readonly IBaseRepository<TradersPayChannel> _tPayChannelRepost;
        /// <summary>
        /// 商家门店
        /// </summary>
        private readonly IBaseRepository<TradersStore> _tStoreRepost;
        /// <summary>
        /// 交易支付接口
        /// </summary>
        private readonly IBaseRepository<PayApi> _payApiRepost;
        /// <summary>
        /// 日志记录引擎
        /// </summary>
        private readonly LogEngine _logEngine;
        /// <summary>
        /// 构造函数，并初始化仓储对象
        /// </summary>
        public PayService()
        {
            _tradeOrderRepost = new BaseRepository<TradeOrder>();
            _tradeResultRepost = new BaseRepository<TradeResult>();
            _tradeRepost = new BaseRepository<Traders>();
            _tPayLicenseRepost = new BaseRepository<PayLicense>();
            _tBankAccountRepost = new BaseRepository<BankAccount>();
            _tPaySecretKeyRepost = new BaseRepository<TradersPaySecretKey>();
            _tPayChannelRepost = new BaseRepository<TradersPayChannel>();
            _tStoreRepost = new BaseRepository<TradersStore>();
            _payApiRepost = new BaseRepository<PayApi>();
            _logEngine = new LogEngine();
        }


        /// <summary>
        /// 检查商户通道是否可用
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public QctPayReturn CheckMerchAccess(int cid, string method, decimal version)
        {
            var payLicense = GetPayLicense(cid, (short)TraderPayLicenseState.Audited);
            if (payLicense == null)
                return QctPayReturn.Fail(code: PayConst.FAIL_CODE_20001, msg: "请先申请支付许可");
            var merchBankAccount = GetBankAccount(cid, (short)TraderBalanceAccountState.Enabled);
            if (merchBankAccount == null)
                return QctPayReturn.Fail(code: PayConst.FAIL_CODE_20001, msg: "您的商户结算账户不可用，请先确保商户结算账户可用再提交支付订单");
            var merchChannel = GetTraderPaySecretKeyAndChannel(cid, (short)TraderPayCchannelState.Enabled);
            if (merchChannel == null)
                return QctPayReturn.Fail(code: PayConst.FAIL_CODE_20001, msg: "您的商户不可用，请先确认是否已经成功申请商户");
            var payApiObj = GetPayApi(merchChannel.ChannelNo, method, version);
            if (payApiObj == null)
                return QctPayReturn.Fail(code: PayConst.FAIL_CODE_40001, msg: "非法支付接口参数名");
            var merchObj = GetMerchByID(cid, payApiObj);
            if (merchObj == null)
                return QctPayReturn.Fail(code: PayConst.FAIL_CODE_20001, msg: "找不到对应商户密钥配置信息，请先申请开通商户");
            merchObj.SourceType = payLicense.SourceType;
            merchObj.ApiNo = payApiObj.ApiNo;
            merchObj.ApiUrl = payApiObj.ApiUrl;
            merchObj.Method = payApiObj.Method;
            merchObj.OptType = payApiObj.OptType;
            return QctPayReturn.Success(data: merchObj);
        }
        /// <summary>
        /// 检查商户是否许可
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public PayLicense GetPayLicense(int cid, short state)
        {
            var obj = _tPayLicenseRepost.GetQuery(o => o.CID == cid && o.State == (short)TraderPayLicenseState.Audited).FirstOrDefault();
            return obj;
        }
        /// <summary>
        /// 获取可用的商户结算账户
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public BankAccount GetBankAccount(int cid, short state)
        {
            var obj = _tBankAccountRepost.GetQuery(o => o.CID == cid && o.State == state).FirstOrDefault();
            return obj;
        }
        /// <summary>
        /// 获取可用的商户支付账号及通道信息
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public TradersPaySecretKey GetTraderPaySecretKeyAndChannel(int cid, short state)
        {
            var obj = _tPaySecretKeyRepost.GetQuery(o => o.CID == cid && o.State == state).FirstOrDefault();
            return obj;
        }
        /// <summary>
        /// 根据接口参数名获得接口实体对象
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public PayApi GetPayApi(int channelNo, string method, decimal version)
        {
            var entity = _payApiRepost.GetQuery(o => o.ChannelNo == channelNo && o.Method == method && o.Version == version && o.State == (short)PayApiState.HasReleased).FirstOrDefault();
            return entity;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tradeOrder"></param>
        /// <returns></returns>
        public bool SaveTradeOrder(TradeOrder tradeOrder)
        {
            using (var trans = new EFDbContext().Database.BeginTransaction())
            {
                _tradeOrderRepost.Add(tradeOrder, false);
                var result = _tradeOrderRepost.SaveChanges();
                return result;
            }
        }
        /// <summary>
        /// 保存商家收款扫码结果
        /// </summary>
        /// <returns></returns>
        public bool SaveMerchScanResult(SxfScanPayResponse sxfResult, out TradeOrder order)
        {
            var tradeResult = new TradeResult(sxfResult);
            var succ = SaveMchTradeResult(tradeResult, out order);
            return succ;
        }
        /// <summary>
        /// 保存购买者付款扫码返回结果
        /// </summary>
        /// <param name="tradeResult"></param>
        /// <param name="tradeOrder"></param>
        /// <returns></returns>
        public bool SaveMchTradeResult(TradeResult tradeResult, out TradeOrder tradeOrder)
        {
            tradeOrder = null;
            var result = false;
            var tradeResultObj = _tradeResultRepost.GetQuery(o => o.OutTradeNo == tradeResult.OutTradeNo && o.MchId3 == tradeResult.MchId3 && o.StoreId3 == tradeResult.StoreId3).FirstOrDefault();
            using (var trans = new EFDbContext().Database.BeginTransaction())
            {
                try
                {
                    if (tradeResultObj != null)
                    {
                        tradeResult.ToCopyProperty(tradeResultObj, new List<string>() { "CreateDT", "OutTradeNo", "Id", "MchId3", "StoreId3" });
                    }
                    else
                    {
                        _tradeResultRepost.Add(tradeResult);
                    }

                    //变更TradeOrder数据状态
                    tradeOrder = _tradeOrderRepost.GetQuery(o => o.OutTradeNo == tradeResult.OutTradeNo && o.MchId3 == tradeResult.MchId3 && o.StoreId3 == tradeResult.StoreId3).FirstOrDefault();
                    tradeOrder.ReceiptAmount = tradeResult.ReceiptAmount;
                    tradeOrder.TradeNo3 = tradeResult.TradeNo3;
                    tradeOrder.State = tradeResult.TradeState;
                    tradeOrder.TradeDate = tradeResult.TradeDate;
                    tradeOrder.TradeTime = tradeResult.TradeTime;
                    tradeOrder.UpdateDT = DateTime.Now;
                    tradeOrder.PayChannel = tradeResult.PayChannel;
                    result = _tradeResultRepost.SaveChanges();
                    var result1 = _tradeOrderRepost.SaveChanges();
                }
                catch (Exception ex)
                {
                    trans.Rollback();//回滚事务
                    _logEngine.WriteError(string.Format("保存后台结果通知并更新状态异常执行回滚操作：{0}，参数：{1}，异常信息：{2}", tradeResult.OutTradeNo, tradeResult.ToJson(), ex.Message), ex, LogModule.支付交易);
                    result = false;
                }
            }
            return result;
        }
        /// <summary>
        /// 根据第三方商户号，商户订单号获得唯一订单
        /// </summary>
        /// <param name="mchId3"></param>
        /// <param name="outTradeNo"></param>
        /// <returns></returns>
        public TradeOrder GetTradeOrder(string mchId3, string outTradeNo)
        {
            var trade = _tradeOrderRepost.GetQuery(o => o.MchId3 == mchId3 && o.OutTradeNo == outTradeNo).FirstOrDefault();
            return trade;
        }
        /// <summary>
        /// 获取最大订单交易流水号
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public string GetMaxTradeNo(string prefix)
        {
            var result = _tradeOrderRepost.GetQuery(o => o.TradeNo.StartsWith(prefix)).Max(o => o.TradeNo);
            return result;
        }

        #region 商户、商户门店、收单渠道、商户收单渠道费率信息
        /// <summary>
        /// 根据商户ID及收单渠道编号获取商户信息 fishtodo:待确认查询接口的操作类型
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="channelNo"></param>
        /// <returns></returns>
        //public MerchantChannelModel GetMerchByID(int cid, int channelNo, short channelPayMode)
        public MerchantChannelModel GetMerchByID(int cid, PayApi payApi)
        {
            var query = (from tpsk in _tPaySecretKeyRepost.GetQuery()
                         join jtpc in _tPayChannelRepost.GetQuery() on tpsk.TPaySecrectId equals jtpc.TPaySecrectId into itpc
                         from tpc in itpc.DefaultIfEmpty()
                         where tpsk.CID == cid && tpsk.ChannelNo == payApi.ChannelNo && tpsk.State == (short)TraderPayCchannelState.Enabled
                         && tpc.ChannelPayMode == payApi.ChannelPayMode && tpc.State == (short)TraderPayCchannelState.Enabled
                         select new MerchantChannelModel()
                         {
                             MchId = (int)tpsk.CID,
                             SecretKey = tpsk.SecretKey,
                             MerchId3 = tpsk.MchId3,
                             SecretKey3 = tpsk.SecretKey3,
                             ChannelNo = tpsk.ChannelNo,
                             PayNotifyUrl = tpc.PayNotifyUrl,
                             RfdNotifyUrl = tpc.RfdNotifyUrl,
                             ChannelPayMode = payApi.ChannelPayMode
                         });
            return query.FirstOrDefault();
        }
        /// <summary>
        /// 获取可用的支付商户
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public TradersPaySecretKey GetMerchByCID(int cid)
        {
            var query = (from tpsk in _tPaySecretKeyRepost.GetQuery()
                         where tpsk.CID == cid && tpsk.State == (short)TraderPayCchannelState.Enabled
                         select tpsk);
            return query.FirstOrDefault();
        }
        /// <summary>
        /// 根据商户及门店号获得商户门店信息
        /// </summary>
        /// <param name="merchObj"></param>
        /// <param name="sid"></param>
        /// <returns></returns>
        public MerchantStoreChannelModel GetMerchStore(MerchantChannelModel merchObj, string sid)
        {
            var query = from tstore in _tStoreRepost.GetQuery()
                        where tstore.CID == merchObj.MchId && tstore.StoreNum == sid && tstore.State == (short)TraderStoreState.Enabled
                        select new MerchantStoreChannelModel()
                        {
                            SID = tstore.StoreNum,
                            StoreId3 = tstore.StoreNum3
                        };
            var store = query.FirstOrDefault();
            if (store != null)
            {
                var obj = new MerchantStoreChannelModel(merchObj);
                obj.SID = store.SID;
                obj.StoreId3 = store.StoreId3;
                return obj;
            }
            return null;
        }
        /// <summary>
        /// 根据CID、SID获取商户门店信息
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="sid"></param>
        public BaseMerchStoreModel GetMerchStore(int cid, string sid)
        {
            var query = from s in _tStoreRepost.GetQuery()
                        join t in _tradeRepost.GetQuery() on s.CID equals t.CID
                        where s.CID == cid && s.StoreNum == sid && s.State == (short)TraderStoreState.Enabled && t.Status == (short)TraderState.Audited
                        select new BaseMerchStoreModel()
                        {
                            Mch_Id = s.CID,
                            Mch_Title = t.Title,
                            Store_Id = s.StoreNum,
                            Store_Name = s.StoreName
                        };
            var model = query.FirstOrDefault();
            if (model == null)
                model = new BaseMerchStoreModel();
            return model;
        }
        /// <summary>
        /// 判断是否存在商户订单
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="outTradeNo"></param>
        /// <returns></returns>
        public bool CanCreateOrder(int cid, string outTradeNo)
        {
            var order = _tradeOrderRepost.GetQuery(o => o.CID == cid && o.OutTradeNo == outTradeNo).FirstOrDefault();
            if (order == null)
                return true;
            else
                return false;
        }
        #endregion
    }
}
