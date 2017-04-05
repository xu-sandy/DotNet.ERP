
namespace Pharos.POS.Retailing.Models.ApiParams
{
    public class LoginInfo : BaseApiParams
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码(md5)
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 设备编码（全局唯一） 
        /// </summary>
        public string DeviceSN { get; set; }
        /// <summary>
        /// 设备类型（pc=1;pad=2;mobile=3）
        /// </summary>
        public int DeviceType { get; set; }
        /// <summary>
        /// 入口点（0=posapp;1=mobile app）
        /// </summary>
        public int EntryPoint { get; set; }

        /// <summary>
        /// 进入练习模式
        /// </summary>
        public bool InTestMode { get; set; }

        public bool IsLock { get; set; }
    }
}
