using System;

namespace Pharos.Logic.ApiData.Pos.Sale.Payment
{
    /// <summary>
    /// 后台发起支付需轮询等待状态返回
    /// </summary>
    public interface IBackgroundPayment : IPay
    {
        /// <summary>
        /// 支付请求提交时间
        /// </summary>
        DateTime RequestPayDate { get; set; }
        /// <summary>
        /// 提交支付请求
        /// </summary>
        /// <returns></returns>
        object RequestPay();
        /// <summary>
        /// 获取支付状态
        /// </summary>
        /// <returns></returns>
        ThirdPartyPaymentStatus GetPayStatus();
        /// <summary>
        /// 测试支付接口网络
        /// </summary>
        /// <returns></returns>
        bool ConnectTest();
    }
   
}
