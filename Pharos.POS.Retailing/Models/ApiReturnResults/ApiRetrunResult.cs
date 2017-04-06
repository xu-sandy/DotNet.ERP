
namespace Pharos.POS.Retailing.Models.ApiReturnResults
{
    /// <summary>
    /// api返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Ex"></typeparam>
    public class ApiRetrunResult<T, Ex> : IApiRetrunResult
    {
        /// <summary>
        /// 接口返回状态码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 接口返回信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 接口相应结果
        /// </summary>
        public T Result { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public Ex ErrorInfo { get; set; }
    }
    public class ApiRetrunResult<T> : IApiRetrunResult
    {
        /// <summary>
        /// 接口返回状态码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 接口返回信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 接口相应结果
        /// </summary>
        public T Result { get; set; }

    }

    public interface IApiRetrunResult
    {
        string Code { get; set; }

        string Message { get; set; }
    }
}
