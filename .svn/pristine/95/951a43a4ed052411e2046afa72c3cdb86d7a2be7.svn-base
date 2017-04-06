using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity
{
    public class ApiLibrary : BaseEntity
    {

        /// <summary>
        /// 接口类型（ 1:支付接口）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short ApiType { get; set; }

        /// <summary>
        /// 接口名称
        /// [长度：20]
        /// [不允许为空]
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 接口代码（全局唯一）
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int ApiCode { get; set; }

        /// <summary>
        /// 接口地址
        /// [长度：500]
        /// [不允许为空]
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// 接口ICON
        /// [长度：200]
        /// [允许为空]
        /// </summary>
        public string ApiIcon { get; set; }

        /// <summary>
        /// 接口顺序
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public int ApiOrder { get; set; }

        /// <summary>
        /// 备注
        /// [长度：200]
        /// [允许为空]
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 请求方式[1:post、2:get]
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short ReqMode { get; set; }

        /// <summary>
        /// 状态（ 0:禁用、 1:可用）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public short State { get; set; }

        /// <summary>
        /// Token
        /// [长度：100]
        /// [允许为空]
        /// </summary>
        public string ApiToken { get; set; }

        /// <summary>
        /// 账号
        /// [长度：50]
        /// [允许为空]
        /// </summary>
        public string ApiAccount { get; set; }

        /// <summary>
        /// 密码
        /// [长度：50]
        /// [允许为空]
        /// </summary>
        public string ApiPwd { get; set; }
        /// <summary>
        /// api不可用图标
        /// </summary>
        public string ApiCloseIcon { get; set; }

        
    }
}
