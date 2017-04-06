using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCT.Pay.Common
{
    #region QctPay
    /// <summary>
    /// 商户退款状态
    /// </summary>
    public enum RefundState : short
    {
        /// <summary>
        /// 退款中
        /// </summary>
        [Description("退款中")]
        RefundIng = 0,
        /// <summary>
        /// 退款成功
        /// </summary>
        [Description("退款成功")]
        RefundSuccess = 1,
        /// <summary>
        /// 退款失败
        /// </summary>
        [Description("退款失败")]
        RefundFail = 2
    }
    /// <summary>
    /// 商户支付交易订单类型（收支）
    /// </summary>
    public enum QctTradeType : short
    {
        /// <summary>
        /// 收入
        /// </summary>
        [Description("收入")]
        Income = 0,
        /// <summary>
        /// 支出
        /// </summary>
        [Description("支出")]
        Expense = 1
    }
    /// <summary>
    /// 币种
    /// </summary>
    public enum PayFeeType : short
    {
        /// <summary>
        /// 人民币
        /// </summary>
        [Description("人民币")]
        RMB = 1
    }
    /// <summary>
    /// 所属体系
    /// </summary>
    public enum PaySourceType : short
    {
        /// <summary>
        /// 云平台
        /// </summary>
        [Description("云平台")]
        CloudQctErp = 1,
        /// <summary>
        /// 外部
        /// </summary>
        [Description("外部")]
        Outside = 2
    }
    /// <summary>
    /// 支付渠道
    /// </summary>
    public enum PayChannel : short
    {
        [Description("无")]
        REFUND = 0,
        /// <summary>
        /// 微信支付
        /// </summary>
        [Description("微信支付")]
        WX = 1,
        /// <summary>
        /// 支付宝支付
        /// </summary>
        [Description("支付宝支付")]
        ALIPAY = 2
    }

    /// <summary>
    /// 支付订单状态
    /// </summary>
    public enum PayState : short
    {
        /// <summary>
        /// 未支付，付款中
        /// </summary>
        [Description("未支付")]
        NotPay = 0,
        /// <summary>
        /// 支付成功
        /// </summary>
        [Description("支付成功")]
        PaySuccess = 1,
        /// <summary>
        /// 支付失败
        /// </summary>
        [Description("支付失败")]
        PayFail = 2,
        /// <summary>
        /// 已撤销
        /// </summary>
        [Description("已撤销")]
        PayCancel = 3,
        /// <summary>
        /// 支付超时
        /// </summary>
        [Description("支付超时")]
        PayTimeout = 4
    }
    #endregion

    #region PayApi
    /// <summary>
    /// 收单渠道状态
    /// </summary>
    public enum PayChannelState : short
    {
        /// <summary>
        /// 未审核
        /// </summary>
        [Description("未审核")]
        NotAuditing = 0,
        /// <summary>
        /// 可用
        /// </summary>
        [Description("可用")]
        Enabled = 1,
        /// <summary>
        /// 停用
        /// </summary>
        [Description("停用")]
        Disabled = 2,
        /// <summary>
        /// 注销
        /// </summary>
        [Description("注销")]
        Cancel = 3
    }
    /// <summary>
    /// 交易支付接口状态
    /// </summary>
    public enum PayApiState : short
    {
        /// <summary>
        /// 未发布
        /// </summary>
        [Description("未发布")]
        NotReleased = 0,
        /// <summary>
        /// 已发布
        /// </summary>
        [Description("已发布")]
        HasReleased = 1,
        /// <summary>
        /// 已失效
        /// </summary>
        [Description("已失效")]
        Expired = 2
    }
    /// <summary>
    /// 支付操作类型
    /// </summary>
    public enum PayOperateType : short
    {
        /// <summary>
        /// 不限
        /// </summary>
        [Description("不限")]
        Unlimited = 0,
        /// <summary>
        /// 收款
        /// </summary>
        [Description("收款")]
        Receipt = 1,
        /// <summary>
        /// 退款
        /// </summary>
        [Description("退款")]
        Refund = 2,
        /// <summary>
        /// 转账
        /// </summary>
        [Description("转账")]
        Transfer = 3,
        /// <summary>
        /// 付款
        /// </summary>
        [Description("付款")]
        Payment = 4,
        /// <summary>
        /// 查询
        /// </summary>
        [Description("查询")]
        Query = 5
    }
    /// <summary>
    /// 交易方式
    /// </summary>
    public enum PayTradeMode
    {
        /// <summary>
        /// 主动扫码（动态）
        /// </summary>
        [Description("购买者扫码(动态)")]
        ActiveScanDyna = 1,
        /// <summary>
        /// 被动扫码
        /// </summary>
        [Description("商家扫码")]
        UnActiveScan = 2,
        /// <summary>
        /// 静态扫码
        /// </summary>
        [Description("购买者扫码(静态)")]
        StaticScan = 3,
        /// <summary>
        /// 银联卡
        /// </summary>
        [Description("银联卡")]
        UnionPay = 4,
        /// <summary>
        /// 会员储值卡
        /// </summary>
        [Description("会员储值卡")]
        MemberCard = 5,
        /// <summary>
        /// 站内转账
        /// </summary>
        [Description("站内转账")]
        SystemTransfer = 6,
        /// <summary>
        /// 退款
        /// </summary>
        [Description("退款")]
        RefundOrder = 7,
        /// <summary>
        /// 其它操作，如订单查询等
        /// </summary>
        [Description("其它")]
        Other = 99
    }
    /// <summary>
    /// 渠道支付方式
    /// </summary>
    public enum ChannelPayMode {
        /// <summary>
        /// 扫码支付
        /// </summary>
        [Description("扫码支付")]
        ScanPay = 1,
        /// <summary>
        /// 网站支付
        /// </summary>
        [Description("网站支付")]
        SitePay = 2,
        /// <summary>
        /// 刷卡支付
        /// </summary>
        [Description("刷卡支付")]
        CardPay = 3
    }

    /// <summary>
    /// Item可以状态 1：可用；2：不可用
    /// </summary>
    public enum DataItemState
    {
        /// <summary>
        /// 可用
        /// </summary>
        [Description("可用")]
        Enabled = 0,
        /// <summary>
        /// 停用
        /// </summary>
        [Description("不可用")]
        Disabled = 1
    }
    #endregion
}
