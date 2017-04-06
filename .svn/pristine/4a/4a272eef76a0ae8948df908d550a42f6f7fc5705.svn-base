using Pharos.Utility;
using System;

namespace Pharos.Logic.BLL.DataSynchronism.Dtos
{
    /// <summary>
    /// 店员信息表
    /// </summary>
    [Excel("店员信息表")]
    public partial class SysStoreUserInfoForLocal
    {


        /// <summary>
        /// 用户ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [Excel("用户ID", 1)]
        [ExcelField(@"^[\s,\S]{1,40}$###用户长度应在1-40位或为空")]
        public string UID { get; set; }

        /// <summary>
        /// 姓名
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        [ExcelField(@"^[\s,\S]{1,50}$###姓名长度应在1-40位或为空")]
        [Excel("姓名", 2)]
        public string FullName { get; set; }

        
        /// <summary>
        /// 员工编号
        /// [长度：100]
        /// [不允许为空]
        /// </summary>
        [Excel("员工编号", 3)]
        [ExcelField(@"^[\s,\S]{1,100}$###姓名长度应在1-100位或为空")]
        public string UserCode { get; set; }

        /// <summary>
        /// 登录密钥
        /// [长度：100]
        /// [不允许为空]
        /// </summary>
        [Excel("登录密钥", 4)]
        [ExcelField(@"^[\s,\S]{1,100}$###登录密钥长度应在1-100位或为空")]
        public string LoginPwd { get; set; }

        /// <summary>
        /// 性别（ 0:女、 1:男）
        /// [长度：1]
        /// [默认值：((1))]
        /// </summary>
        [Excel("性别", 5)]
        [ExcelField(@"^(true|TRUE|false|FALSE)$###性别应为TRUE|FALSE")]
        public bool Sex { get; set; }

        /// <summary>
        /// 本店角色（ 1:店长、 2:营业员、 3:仓管员）（多个 ID以,号间隔）
        /// [长度：2000]
        /// </summary>
        [Excel("本店角色", 6)]
        [ExcelField(@"^[\s,\S]{0,2000}$###本店角色长度应在0-2000位")]
        public string OperateAuth { get; set; }
        /// <summary>
        /// 最近登录时间
        /// [不允许为空]
        /// [默认值：(((getdate()) ))]
        /// </summary>
        [Excel("最近登录时间", 7)]
        [ExcelField(@"^(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}){1,20}$###最近登录时间格式不正确(yyyy-MM-dd HH:mm:ss)或为空")]
        public DateTime LoginDT { get; set; }

        
    }
}
