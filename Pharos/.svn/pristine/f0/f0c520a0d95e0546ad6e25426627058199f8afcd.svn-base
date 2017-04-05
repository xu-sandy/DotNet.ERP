// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的所有集成外部接口信息（如：银联支付接口、扫码支付接口）
// --------------------------------------------------

using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Utility;
using System;

namespace Pharos.Logic.LocalEntity
{
    /// <summary>
    /// 集成接口库
    /// </summary>
    [Excel("接口信息")]

    public class ApiLibrary : BaseEntity
    {
        public Int64 Id { get; set; }
        [ExcelField(@"^[0-9]{1,5}$###接口类型长度应在1-5位且为整数")]
        [Excel("接口类型", 1)]
        /// <summary>
        /// 接口类型（ 1:支付接口）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short ApiType { get; set; }
        [ExcelField(@"^[\s,\S]{1,20}$###接口名称长度应在1-20位")]
        [Excel("接口名称", 2)]
        /// <summary>
        /// 接口名称
        /// [长度：20]
        /// [不允许为空]
        /// </summary>
        public string Title { get; set; }
        [ExcelField(@"^[0-9]{1,10}$###接口代码长度应在1-10位且为整数")]
        [LocalKey]
        [Excel("接口代码", 3)]
        /// <summary>
        /// 接口代码（全局唯一）
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int ApiCode { get; set; }
        [ExcelField(@"^[\s,\S]{1,500}$###接口地址长度应在1-500位")]

        [Excel("接口地址", 4)]
        /// <summary>
        /// 接口地址
        /// [长度：500]
        /// [不允许为空]
        /// </summary>
        public string ApiUrl { get; set; }
        [ExcelField(@"^[\s,\S]{0,200}$###接口ICON长度超过200位")]
        [Excel("接口ICON", 5)]
        /// <summary>
        /// 接口ICON
        /// [长度：200]
        /// [允许为空]
        /// </summary>
        public string ApiIcon { get; set; }
        [ExcelField(@"^[0-9]{1,10}$###接口顺序长度应在1-10位")]

        [Excel("接口顺序", 6)]
        /// <summary>
        /// 接口顺序
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public int ApiOrder { get; set; }
        [ExcelField(@"^[\s,\S]{0,200}$###备注长度超过200位")]
        [Excel("备注", 7)]
        /// <summary>
        /// 备注
        /// [长度：200]
        /// [允许为空]
        /// </summary>
        public string Memo { get; set; }
        [ExcelField(@"^[1,2]{1}$###请求方式应为1或者是2且不能为空")]
        [Excel("请求方式", 8)]
        /// <summary>
        /// 请求方式[1:post、2:get]
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short ReqMode { get; set; }
        [ExcelField(@"^[0,1]{1}$###请求方式应为0或者是1且不能为空")]

        [Excel("状态", 9)]
        /// <summary>
        /// 状态（ 0:禁用、 1:可用）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public short State { get; set; }
        [ExcelField(@"^[\s,\S]{0,100}$###Token长度超过100位")]

        [Excel("Token", 10)]
        /// <summary>
        /// Token
        /// [长度：100]
        /// [允许为空]
        /// </summary>
        public string ApiToken { get; set; }
        [ExcelField(@"^[\s,\S]{0,50}$###账号长度超过50位")]
        [Excel("账号", 11)]
        /// <summary>
        /// 账号
        /// [长度：50]
        /// [允许为空]
        /// </summary>
        public string ApiAccount { get; set; }
        [ExcelField(@"^[\s,\S]{0,50}$###密码长度超过50位")]
        [Excel("密码", 12)]
        /// <summary>
        /// 密码
        /// [长度：50]
        /// [允许为空]
        /// </summary>
        public string ApiPwd { get; set; }
    }
}
