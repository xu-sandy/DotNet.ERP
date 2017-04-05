﻿using Newtonsoft.Json;
using Pharos.Logic.OMS.Entity;
using QCT.Pay.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Pharos.Logic.OMS.Models
{
    #region Sxf Base
    /// <summary>
    /// 请求协议参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class SxfBaseTradeRequest
    {
        public SxfBaseTradeRequest() { }
        /// <summary>
        /// 构造商户基础信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="trade"></param>
        /// <returns></returns>
        public SxfBaseTradeRequest(TradeOrder order)
        {
            MerchantId = order.MchId3;
            ShopId = order.StoreId3;
            TerminalId = order.DeviceId3;
        }

        /// <summary>
        /// 字符集，只能取以下枚举值
        /// 00--GBK
        /// 01--GB2312
        /// 02--UTF-8
        /// 默认02--UTF-8
        /// </summary>
        [JsonProperty("charSet")]
        public string CharSet { get; set; }
        /// <summary>
        /// 接口类型
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        /// <summary>
        /// 商户编号，无卡支付平台给接入方分配的唯一标识
        /// </summary>
        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }
        /// <summary>
        /// 门店编号
        /// </summary>
        [JsonProperty("shopId")]
        public string ShopId { get; set; }
        /// <summary>
        /// 终端编号
        /// </summary>
        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }
        /// <summary>
        /// 签名方式，MD5
        /// </summary>
        [JsonProperty("signType")]
        public string SignType { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }
        /// <summary>
        /// 签名数据
        /// </summary>
        [JsonProperty("signature")]
        public string Signature { get; set; }
    }

    /// <summary>
    /// 返回协议参数
    /// </summary>
    [Serializable]
    public class SxfBaseTradeResponse
    {
        #region Properties
        /// <summary>
        /// 返回码，00000表示成功，其他均为错误
        /// </summary>
        [JsonProperty("rspCod")]
        public string RspCod { get; set; }
        /// <summary>
        /// 返回信息，SUCCESS表示成功，其他均为错误信息
        /// </summary>
        [JsonProperty("rspMsg")]
        public string RspMsg { get; set; }
        /// <summary>
        /// 接口类型
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        /// <summary>
        /// 商户编号
        /// </summary>
        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }
        /// <summary>
        /// 门店编号
        /// </summary>
        [JsonProperty("shopId")]
        public string ShopId { get; set; }
        /// <summary>
        /// 终端编号
        /// </summary>
        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }
        /// <summary>
        /// 签名方式
        /// </summary>
        [JsonProperty("signType")]
        public string SignType { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }
        /// <summary>
        /// 签名数据
        /// </summary>
        [JsonProperty("signature")]
        public string Signature { get; set; }
        #endregion

        #region Methods
        public bool IsSuccess()
        {
            if (this.RspCod == PayConst.SXF_SUCCESS_CODE)
                return true;
            else
                return false;
        }
        #endregion
    }

    /// <summary>
    /// Sxf 支付交易请求参数
    /// </summary>
    [Serializable]
    public class SxfPayRequest : SxfBaseTradeRequest
    {
        public SxfPayRequest() { }
        public SxfPayRequest(TradeOrder order) :base(order){}

        #region Properties
        /// <summary>
        /// 商户支付订单号，每笔支付订单的唯一标识，商户需保持该字段在系统内唯一
        /// 建议生成规则：时 间 (yyyyMMddHHmmss)+4位序号
        /// </summary>
        [JsonProperty("payOrderNo")]
        public string PayOrderNo { get; set; }
        /// <summary>
        /// 订单类型，1：普通订单 2：当面收款订单
        /// </summary>
        [JsonProperty("orderType")]
        public string OrderType { get; set; }
        /// <summary>
        /// 交易日期，交易日期，格式： YYYYMMDD
        /// </summary>
        [JsonProperty("txDate")]
        public string TxDate { get; set; }
        /// <summary>
        /// 交易金额，以分为单位
        /// </summary>
        [JsonProperty("txAmt")]
        public int TxAmt { get; set; }
        /// <summary>
        /// 支付结果通知地址，支付结果的后台通知地址，如果该值为空，则使用商户开户时默认设置的支付结果通知地址
        /// </summary>
        [JsonProperty("payNotifyUrl")]
        public string PayNotifyUrl { get; set; }
        /// <summary>
        /// 用户手机号，支付用户的手机号，订单类型为 2:当面收款订单，则该值必输
        /// </summary>
        [JsonProperty("userMobile")]
        public string UserMobile { get; set; }
        /// <summary>
        /// 商品名称，不输，则默认为“超市购物”
        /// </summary>
        [JsonProperty("goodsName")]
        public string GoodsName { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        [JsonProperty("goodDesc")]
        public string GoodDesc { get; set; }
        #endregion

        #region Methods
        #endregion
    }

    /// <summary>
    /// Sxf 支付交易响应参数
    /// </summary>
    [Serializable]
    public class SxfPayResponse : SxfBaseTradeResponse
    {
        /// <summary>
        /// 商户支付订单号
        /// </summary>
        [JsonProperty("payOrderNo")]
        public string PayOrderNo { get; set; }
    }
    #endregion

    #region Sxf BuildPayToken
    /// <summary>
    /// Sxf 主扫支付动态二维码生成请求参数
    /// </summary>
    [Serializable]
    public class SxfBuildPayTokenRequest : SxfPayRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public SxfBuildPayTokenRequest() { }
        /// <summary>
        /// 根据TradeOrder构造Sxf主扫支付动态支付订单
        /// </summary>
        /// <param name="order"></param>
        public SxfBuildPayTokenRequest(TradeOrder order, string payNotifyUrl):base(order)
        {
            CharSet = PayConst.SXF_DEF_CHARSET;
            SignType = PayConst.SXF_DEF_SIGNTYPE;
            Version = PayConst.SXF_DEF_VERSION;

            Type = PayConst.SXF_TYPE_BUILDPAYTOKEN;
            PayOrderNo = order.OutTradeNo;
            TxDate = order.CreateDT.ToString("yyyyMMdd");
            TxAmt = PayTradeHelper.FromYuan2Cent(order.TotalAmount);
            UserMobile = order.BuyerMobile;
            PayNotifyUrl = payNotifyUrl;
            OrderType = PayConst.SXF_DEF_ORDERTYPE;
            GoodsName = order.GoodsName;
            GoodDesc = order.GoodsDesc;
        }

        /// <summary>
        /// 支付页面通知地址，支付结果前台通知地址，如果该值为空则跳转到默认成功页面
        /// </summary>
        [JsonProperty("frontNotifyUrl")]
        public string FrontNotifyUrl { get; set; }

    }

    /// <summary>
    /// 主扫支付动态收款二维码信息响应参数
    /// </summary>
    [Serializable]
    public class SxfBuildPayTokenResponse : SxfPayResponse
    {
        /// <summary>
        /// 动态二维码数据，一串支付的 url，请商家自行将该结果生成二维码
        /// </summary>
        [JsonProperty("payToken")]
        public string PayToken { get; set; }
        /// <summary>
        /// 动态二维码的 httpurl
        /// </summary>
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
    }
    #endregion

    #region Sxf ScanPay
    /// <summary>
    /// 被扫支付请求参数
    /// </summary>
    [Serializable]
    public class SxfScanPayRequest : SxfPayRequest
    {
        public SxfScanPayRequest() { }
        /// <summary>
        /// 根据TradeOrder构造Sxf被扫支付订单
        /// </summary>
        /// <param name="order"></param>
        /// <param name="payNotifyUrl"></param>
        public SxfScanPayRequest(TradeOrder order, string payNotifyUrl):base(order)
        {
            CharSet = PayConst.SXF_DEF_CHARSET;
            SignType = PayConst.SXF_DEF_SIGNTYPE;
            Version = PayConst.SXF_DEF_VERSION;

            Type = PayConst.SXF_TYPE_SCANPAY;
            PayOrderNo = order.OutTradeNo;
            TxDate = order.CreateDT.ToString("yyyyMMdd");
            UserPayToken = order.BuyerPayToken;
            TxAmt = Convert.ToInt32(order.TotalAmount * PayConst.YUAN_2_CENT_RATE);
            UserPayToken = order.BuyerPayToken;
            UserMobile = order.BuyerMobile;
            PayNotifyUrl = payNotifyUrl;
            OrderType = PayConst.SXF_DEF_ORDERTYPE;
            GoodsName = order.GoodsName;
            GoodDesc = order.GoodsDesc;
        }

        /// <summary>
        /// 购买者的支付二维码字符串
        /// </summary>
        [JsonProperty("userPayToken")]
        public string UserPayToken { get; set; }
    }

    /// <summary>
    /// Sxf 被扫支付响应参数
    /// </summary>
    [Serializable]
    public class SxfScanPayResponse : SxfPayResponse
    {
        /// <summary>
        /// 交易金额，以分为单位
        /// </summary>
        [JsonProperty("txAmt")]
        public int TxAmt { get; set; }
        /// <summary>
        /// 支付结果，S：支付成功 F：支付失败
        /// </summary>
        [JsonProperty("payResult")]
        public string PayResult { get; set; }
        /// <summary>
        /// 支付渠道，WX：微信支付，ALIPAY：支付宝支付
        /// </summary>
        [JsonProperty("payChannel")]
        public string PayChannel { get; set; }
    }
    #endregion

    #region Sxf RefundApply
    /// <summary>
    /// 退款申请请求参数
    /// </summary>
    [Serializable]
    public class SxfRefundApplyRequest : SxfBaseTradeRequest
    {
        public SxfRefundApplyRequest() { }
        /// <summary>
        /// 根据TradeOrder构造退款申请订单
        /// </summary>
        /// <param name="order"></param>
        /// <param name="rfdNotifyUrl"></param>
        public SxfRefundApplyRequest(TradeOrder order, string rfdNotifyUrl):base(order)
        {
            CharSet = PayConst.SXF_DEF_CHARSET;
            SignType = PayConst.SXF_DEF_SIGNTYPE;
            Version = PayConst.SXF_DEF_VERSION;

            Type = PayConst.SXF_TYPE_RFDAPPLY;
            OutTradeNo = order.OutTradeNo;
            OldOutTradeNo = order.OldOutTradeNo;
            RefundAmount = Convert.ToInt32(order.TotalAmount * PayConst.YUAN_2_CENT_RATE);
            RefundReason = order.RefundReason;
            RfdNotifyUrl = rfdNotifyUrl;
        }

        /// <summary>
        /// 商户退款订单号，每笔退款订单的唯一标识，商户需保持该字段在系统内唯一
        /// </summary>
        [JsonProperty("rfdOrderNo")]
        public string OutTradeNo { get; set; }
        /// <summary>
        /// 原商户支付订单号
        /// </summary>
        [JsonProperty("payOrderNo")]
        public string OldOutTradeNo { get; set; }
        /// <summary>
        /// 退款金额，以分为单位
        /// </summary>
        [JsonProperty("rfdAmt")]
        public int RefundAmount { get; set; }
        /// <summary>
        /// 退款结果通知地址，如果该值为空，则使用商户开户时默认设置的退款结果通知地址
        /// </summary>
        [JsonProperty("rfdNotifyUrl")]
        public string RfdNotifyUrl { get; set; }
        /// <summary>
        /// 退款原由
        /// </summary>
        [JsonProperty("rfdReason")]
        public string RefundReason { get; set; }
    }

    /// <summary>
    /// 退款申请响应参数
    /// </summary>
    [Serializable]
    public class SxfRefundApplyResponse : SxfBaseTradeResponse
    {
        /// <summary>
        /// 商户退款订单号，每笔退款订单的唯一标识，商户需保持该字段在系统内唯一
        /// </summary>
        [JsonProperty("rfdOrderNo")]
        public string OutRefundNo { get; set; }
        /// <summary>
        /// 退款金额，以分为单位
        /// </summary>
        [JsonProperty("rfdAmt")]
        public int RefundAmount { get; set; }
        /// <summary>
        /// 退款结果，商家如果收到此字段值为"0"，说明该退款订单还处于退款中，请等待退款结果后台通知，以确认是否退款成功
        /// 0：退款中;1：退款成功;2：退款失败
        /// </summary>
        [JsonProperty("rfdResult")]
        public string RefundResult { get; set; }
    }
    #endregion

    #region Sxf Notify
    /// <summary>
    /// 支付结果后台通知请求参数
    /// </summary>
    [Serializable]
    public class SxfPayNotifyRequest : SxfBaseTradeRequest
    {
        /// <summary>
        /// 商户支付订单号
        /// </summary>
        [JsonProperty("payOrderNo")]
        public string PayOrderNo { get; set; }
        /// <summary>
        /// 交易金额，以分为单位
        /// </summary>
        [JsonProperty("payAmt")]
        public int PayAmt { get; set; }
        /// <summary>
        /// 支付结果，S：支付成功 F：支付失败
        /// </summary>
        [JsonProperty("payResult")]
        public string PayResult { get; set; }
        /// <summary>
        /// 支付渠道，WX：微信支付，ALIPAY：支付宝支付
        /// </summary>
        [JsonProperty("payChannel")]
        public string PayChannel { get; set; }
        /// <summary>
        /// 支付日期，支付完成日期，格式yyyyMMdd
        /// </summary>
        [JsonProperty("payDate")]
        public string PayDate { get; set; }
        /// <summary>
        /// 支付时间,支付完成时间，格式 HHmmss
        /// </summary>
        [JsonProperty("payTime")]
        public string PayTime { get; set; }
    }

    /// <summary>
    /// 退款结果后台通知请求参数
    /// </summary>
    [Serializable]
    public class SxfRefundNotifyRequest : SxfBaseTradeRequest
    {
        /// <summary>
        /// 商户退款订单号
        /// </summary>
        [JsonProperty("rfdOrderNo")]
        public string RfdOrderNo { get; set; }
        /// <summary>
        /// 退款金额，以分为单位
        /// </summary>
        [JsonProperty("rfdAmt")]
        public int RfdAmt { get; set; }
        /// <summary>
        /// 退款结果，S：支付成功 F：支付失败
        /// </summary>
        [JsonProperty("rfdResult")]
        public string RfdResult { get; set; }
        /// <summary>
        /// 支付渠道，WX：微信支付，ALIPAY：支付宝支付
        /// </summary>
        [JsonProperty("payChannel")]
        public string PayChannel { get; set; }
        /// <summary>
        /// 退款日期，支付完成日期，格式yyyyMMdd
        /// </summary>
        [JsonProperty("rfdDate")]
        public string RfdDate { get; set; }
        /// <summary>
        /// 退款时间,支付完成时间，格式 HHmmss
        /// </summary>
        [JsonProperty("rfdTime")]
        public string RfdTime { get; set; }
    }
    #endregion

    #region Sxf Query
    /// <summary>
    /// 单笔支付订单查询请求参数
    /// </summary>
    [Serializable]
    public class SxfPayQueryRequest : SxfBaseTradeRequest
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="req"></param>
        /// <param name="merchStore"></param>
        public SxfPayQueryRequest(PayQueryRequest req, MerchantStoreChannelModel merchStore)
        {
            this.CharSet = PayConst.SXF_DEF_CHARSET;
            this.MerchantId = merchStore.MerchId3;
            this.ShopId = merchStore.StoreId3;
            this.TerminalId = req.Device_Id.ToString();
            this.SignType = PayConst.SXF_DEF_SIGNTYPE;
            this.Type = PayConst.SXF_TYPE_PAYORDERQUERY;
            this.PayOrderNo = req.Out_Trade_No;
            this.Version = PayConst.SXF_DEF_VERSION;
        }

        #region Properties
        /// <summary>
        /// 商户支付订单号，与请参数中数据一致，原样返回
        /// </summary>
        [JsonProperty("payOrderNo")]
        public string PayOrderNo { get; set; }
        #endregion

        #region Methods
        #endregion
    }
    /// <summary>
    /// 单笔支付订单查询响应参数
    /// </summary>
    [Serializable]
    public class SxfPayQueryResponse : SxfBaseTradeResponse
    {
        #region Properties
        /// <summary>
        /// 交易金额
        /// </summary>
        [JsonProperty("txAmt")]
        public int TxAmt { get; set; }
        /// <summary>
        /// 支付订单状态，支付订单状态：U：未支付； S：支付成功；F：支付失败； C：已撤销；T：支付超时
        /// </summary>
        [JsonProperty("payStatus")]
        public string PayStatus { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        [JsonProperty("orderType")]
        public string OrderType { get; set; }
        /// <summary>
        /// 用户手机号
        /// </summary>
        [JsonProperty("userMobile")]
        public string UserMobile { get; set; }
        /// <summary>
        /// 支付日期，支付完成日期，格式yyyyMMdd
        /// </summary>
        [JsonProperty("payDate")]
        public string PayDate { get; set; }
        /// <summary>
        /// 支付时间,支付完成时间，格式 HHmmss
        /// </summary>
        [JsonProperty("payTime")]
        public string PayTime { get; set; }
        /// <summary>
        /// 支付渠道，WX：微信支付，ALIPAY：支付宝支付
        /// </summary>
        [JsonProperty("payChannel")]
        public string PayChannel { get; set; }
        /// <summary>
        /// 商户支付订单号
        /// </summary>
        [JsonProperty("payOrderNo")]
        public string PayOrderNo { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// 转换为单笔支付订单查询响应Model
        /// </summary>
        /// <param name="merchStore"></param>
        /// <returns></returns>
        public PayQueryResponse ToPayQueryResponse(MerchantStoreChannelModel merchStore)
        {
            var obj = new PayQueryResponse()
            {
                Buyer_Mobile = this.UserMobile,
                Mch_Id = merchStore.MchId,
                Device_Id = this.TerminalId,
                Order_Type = this.OrderType,
                Out_Trade_No = this.PayOrderNo,
                Pay_Channel = this.PayChannel,
                Pay_Date = PayTradeHelper.Convert2DateFormat(this.PayDate, this.PayTime, "yyyy-MM-dd HH:mm:ss"),
                Pay_Status = (PayTradeHelper.Convert2EnumString<PayState>(PayTradeHelper.Convert2EnumValue<SxfPayState>(this.PayStatus))).ToUpper(),
                Return_Code = this.RspCod,
                Return_Msg = this.RspMsg,
                Store_Id = merchStore.SID,
                Sign_Type = this.SignType,
                Total_Amount = this.TxAmt,
                Version = PayConst.DEF_VERSION
            };
            return obj;
        }
        #endregion
    }
    /// <summary>
    /// 单笔支付订单查询响应参数
    /// </summary>
    [Serializable]
    public class SxfBasePayQueryResponse
    {
        #region Properties
        /// <summary>
        /// 交易金额
        /// </summary>
        [JsonProperty("txAmt")]
        public int TxAmt { get; set; }
        /// <summary>
        /// 支付订单状态，支付订单状态：U：未支付； S：支付成功；F：支付失败； C：已撤销；T：支付超时
        /// </summary>
        [JsonProperty("payStatus")]
        public string PayStatus { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        [JsonProperty("orderType")]
        public string OrderType { get; set; }
        /// <summary>
        /// 用户手机号
        /// </summary>
        [JsonProperty("userMobile")]
        public string UserMobile { get; set; }
        /// <summary>
        /// 支付日期，支付完成日期，格式yyyyMMdd
        /// </summary>
        [JsonProperty("payDate")]
        public string PayDate { get; set; }
        /// <summary>
        /// 支付时间,支付完成时间，格式 HHmmss
        /// </summary>
        [JsonProperty("payTime")]
        public string PayTime { get; set; }
        /// <summary>
        /// 支付渠道，WX：微信支付，ALIPAY：支付宝支付
        /// </summary>
        [JsonProperty("payChannel")]
        public string PayChannel { get; set; }
        /// <summary>
        /// 商户支付订单号
        /// </summary>
        [JsonProperty("payOrderNo")]
        public string PayOrderNo { get; set; }
        #endregion
    }
    /// <summary>
    /// 支付订单分页查询请求参数
    /// </summary>
    [Serializable]
    public class SxfPayBatchQueryRequest : SxfBaseTradeRequest
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="req"></param>
        /// <param name="merchStore"></param>
        public SxfPayBatchQueryRequest(PayBatchQueryRequest req, MerchantStoreChannelModel merchStore)
        {
            this.CharSet = PayConst.SXF_DEF_CHARSET;
            this.MerchantId = merchStore.MerchId3;
            this.ShopId = merchStore.StoreId3;
            this.TerminalId = req.Device_Id;
            this.SignType = PayConst.SXF_DEF_SIGNTYPE;
            this.Version = PayConst.SXF_DEF_VERSION;
            this.Type = PayConst.SXF_TYPE_PAYORDERPAGEQUERY;
            this.PAG_NO = req.Page_Num;
            this.PER_PAG_CNT = req.Page_Size;
            this.StartDate = req.Start_Date;
            this.EndDate = req.End_Date;
            this.OrderType = req.Order_Type;
        }

        #region Properties
        /// <summary>
        /// 查询起始日期，格式yyyyMMdd
        /// </summary>
        [JsonProperty("startDate")]
        public string StartDate { get; set; }
        /// <summary>
        /// 截止日期，格式yyyyMMdd
        /// </summary>
        [JsonProperty("endDate")]
        public string EndDate { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        [JsonProperty("orderType")]
        public string OrderType { get; set; }
        /// <summary>
        /// 用户手机号
        /// </summary>
        [JsonProperty("userMobile")]
        public string UserMobile { get; set; }
        /// <summary>
        /// 页序号
        /// </summary>
        [JsonProperty("PAG_NO")]
        public int PAG_NO { get; set; }
        /// <summary>
        /// 每页记录数
        /// </summary>
        [JsonProperty("PER_PAG_CNT")]
        public int PER_PAG_CNT { get; set; }
        #endregion
    }
    /// <summary>
    /// 单笔支付订单查询响应参数
    /// </summary>
    [Serializable]
    public class SxfPayBatchQueryResponse : SxfBaseTradeResponse
    {
        #region Properties
        /// <summary>
        /// 查询起始日期，格式yyyyMMdd
        /// </summary>
        [JsonProperty("startDate")]
        public string StartDate { get; set; }
        /// <summary>
        /// 截止日期，格式yyyyMMdd
        /// </summary>
        [JsonProperty("endDate")]
        public string EndDate { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        [JsonProperty("orderType")]
        public string OrderType { get; set; }
        /// <summary>
        /// 用户手机号(what? 批量为何要userMobile)
        /// </summary>
        [JsonProperty("userMobile")]
        public string UserMobile { get; set; }
        /// <summary>
        /// 页序号
        /// </summary>
        [JsonProperty("PAG_NO")]
        public int PAG_NO { get; set; }
        /// <summary>
        /// 每页记录数
        /// </summary>
        [JsonProperty("PER_PAG_CNT")]
        public int PER_PAG_CNT { get; set; }
        /// <summary>
        /// 商户批量查询订单List
        /// </summary>
        [JsonProperty("orderList")]
        public List<SxfBasePayQueryResponse> OrderList { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// 转换为PayBatchQueryResponse
        /// </summary>
        /// <param name="merchStore"></param>
        /// <returns></returns>
        public PayBatchQueryResponse ToPayBatchQueryRsp(MerchantStoreChannelModel merchStore)
        {
            var obj = new PayBatchQueryResponse()
            {
                Buyer_Mobile = this.UserMobile,
                Mch_Id = merchStore.MchId,
                Device_Id = this.TerminalId,
                Order_Type = this.OrderType,
                Return_Code = PayTradeHelper.TransCodeBySxf(this.RspCod),
                Return_Msg = this.RspMsg,
                Store_Id = merchStore.SID,
                Sign_Type = this.SignType,
                Version = PayConst.DEF_VERSION,
                Start_Date = this.StartDate,
                End_Date = this.EndDate,
                Page_Num = this.PAG_NO,
                Page_Size = this.PER_PAG_CNT,
                Order_List = this.ToBasePayQueryRspOrderList(merchStore)
            };
            return obj;
        }
        /// <summary>
        /// 转换为BasePayQueryOrderList
        /// </summary>
        /// <param name="merchStore"></param>
        /// <returns></returns>
        public List<BasePayQueryResponse> ToBasePayQueryRspOrderList(MerchantStoreChannelModel merchStore)
        {
            var orderList = new List<BasePayQueryResponse>();
            if (this.OrderList != null)
            {
                foreach (var item in this.OrderList)
                {
                    orderList.Add(new BasePayQueryResponse()
                    {
                        Buyer_Mobile = item.UserMobile,
                        Order_Type = item.OrderType,
                        Out_Trade_No = item.PayOrderNo,
                        Pay_Channel = item.PayChannel,
                        Pay_Date = PayTradeHelper.Convert2DateFormat(item.PayDate, item.PayTime, "yyyy-MM-dd HH:mm:ss"),
                        Pay_Status = (PayTradeHelper.Convert2EnumString<PayState>(PayTradeHelper.Convert2EnumValue<SxfPayState>(item.PayStatus))).ToUpper(),
                        Total_Amount = item.TxAmt
                    });
                }
            }
            return orderList;
        }
        #endregion
    }

    /// <summary>
    /// 单笔退款订单查询请求参数
    /// </summary>
    [Serializable]
    public class SxfRefundQueryRequest : SxfBaseTradeRequest
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="req"></param>
        /// <param name="merchStore"></param>
        public SxfRefundQueryRequest(RefundQueryRequest req, MerchantStoreChannelModel merchStore)
        {
            this.CharSet = PayConst.SXF_DEF_CHARSET;
            this.MerchantId = merchStore.MerchId3;
            this.ShopId = merchStore.StoreId3;
            this.TerminalId = req.Device_Id.ToString();
            this.SignType = PayConst.SXF_DEF_SIGNTYPE;
            this.Type = PayConst.SXF_TYPE_RFDORDERQUERY;
            this.RfdOrderNo = req.Refund_Trade_No;
            this.Version = PayConst.SXF_DEF_VERSION;
        }

        #region Properties
        /// <summary>
        /// 商户退款订单号，与请参数中数据一致，原样返回
        /// </summary>
        [JsonProperty("rfdOrderNo")]
        public string RfdOrderNo { get; set; }
        #endregion
    }
    /// <summary>
    /// 单笔退款订单查询响应参数
    /// </summary>
    [Serializable]
    public class SxfRefundQueryResponse : SxfBaseTradeResponse
    {
        #region Properties
        /// <summary>
        /// 商户退款订单号
        /// </summary>
        [JsonProperty("rfdOrderNo")]
        public string RfdOrderNo { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        [JsonProperty("rfdAmt")]
        public int RfdAmt { get; set; }
        /// <summary>
        /// 退款订单状态：F：失败； S：成功； U：预登记（退款中）
        /// </summary>
        [JsonProperty("rfdStatus")]
        public string RfdStatus { get; set; }
        /// <summary>
        /// 退款日期，格式yyyyMMdd
        /// </summary>
        [JsonProperty("rfdDate")]
        public string RfdDate { get; set; }
        /// <summary>
        /// 退款时间，格式 HHmmss
        /// </summary>
        [JsonProperty("rfdTime")]
        public string RfdTime { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="merchStore"></param>
        /// <returns></returns>
        public RefundQueryResponse ToRefundQueryResponse(MerchantStoreChannelModel merchStore)
        {
            var obj = new RefundQueryResponse()
            {
                Mch_Id = merchStore.MchId,
                Device_Id = this.TerminalId,
                Refund_Trade_No = this.RfdOrderNo,
                Refund_Date = PayTradeHelper.Convert2DateFormat(this.RfdDate, this.RfdTime, "yyyy-MM-dd HH:mm:ss"),
                Refund_Status = (PayTradeHelper.Convert2EnumString<RefundState>(PayTradeHelper.Convert2EnumValue<SxfRefundState>(this.RfdStatus))).ToUpper(),
                Return_Code = PayTradeHelper.TransCodeBySxf(this.RspCod),
                Return_Msg = this.RspMsg,
                Store_Id = merchStore.SID,
                Sign_Type = this.SignType,
                Refund_Amount = this.RfdAmt,
                Version = PayConst.DEF_VERSION
            };
            return obj;
        }
        #endregion
    }
    #endregion
}
