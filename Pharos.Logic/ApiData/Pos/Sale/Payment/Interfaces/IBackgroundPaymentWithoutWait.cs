
namespace Pharos.Logic.ApiData.Pos.Sale.Payment
{
    /// <summary>
    /// 后台发起支付不需要轮询等待状态返回
    /// </summary>
    public interface IBackgroundPaymentWithoutWait : IPay
    {
        /// <summary>
        /// 提交支付请求
        /// </summary>
        /// <returns></returns>
        object RequestPay();
        /// <summary>
        /// 测试支付接口网络
        /// </summary>
        /// <returns></returns>
        bool ConnectTest();
    }
}
