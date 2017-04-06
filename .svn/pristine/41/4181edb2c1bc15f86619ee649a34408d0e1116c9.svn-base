using Pharos.Logic.BLL.DataSynchronism;
using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Logic.Entity;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.LocalEntity
{
    /// <summary>
    /// 店员信息表
    /// </summary>
    [Excel("店员信息表")]
    public partial class SysStoreUserInfo : BaseEntity, ICanUpdateEntity
    {

        public Int64 Id { get; set; }

        [Excel("用户ID", 1)]
        [ExcelField(@"^[0-9,a-z,A-Z]{1,40}$###用户ID应为Guid")]

        /// <summary>
        /// 用户ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string UID { get; set; }
        [Excel("姓名", 2)]
        [ExcelField(@"^[\s,\S]{1,50}$###姓名为必填且不超过50字符")]

        /// <summary>
        /// 姓名
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        public string FullName { get; set; }
        [Excel("员工编号", 3)]
        [ExcelField(@"^[0-9]{1,100}$###员工编号为数字")]
        [LocalKey]
        /// <summary>
        /// 员工编号
        /// [长度：100]
        /// [不允许为空]
        /// </summary>
        public string UserCode { get; set; }
        [Excel("登录密钥", 4)]
        [ExcelField(@"^[\s,\S]{1,100}$###登录密钥为数字")]

        /// <summary>
        /// 登录密钥
        /// [长度：100]
        /// [不允许为空]
        /// </summary>
        public string LoginPwd { get; set; }
        [Excel("性别", 5)]
        [ExcelField(@"^[0,1]$###性别为值范围为（ 0:女、 1:男）")]

        /// <summary>
        /// 性别（ 0:女、 1:男）
        /// [长度：1]
        /// [默认值：((1))]
        /// </summary>
        public bool Sex { get; set; }
        [Excel("本店角色", 6)]
        [ExcelField(@"^[0-9]{1,2}[,][1,2,3,4](\|[0-9]{1,2}[,][1,2,3,4])*$###本店角色为值范围为本店角色（1:店长、2:营业员、3:收银员、4:数据维护），格式：门店ID,角色ID|门店ID,角色ID ")]

        /// <summary>
        /// 本店角色（ 1:店长、 2:营业员、 3:仓管员）（多个 ID以,号间隔）
        /// [长度：2000]
        /// </summary>
        public string OperateAuth { get; set; }
        [Excel("最近登录时间", 7)]
        [ExcelField(@"^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$###最近登录时间格式为yyyy-MM-dd HH:mm:ss")]


        /// <summary>
        /// 最近登录时间
        /// [不允许为空]
        /// [默认值：(((getdate()) ))]
        /// </summary>
        public DateTime LoginDT { get; set; }

        public bool HasUpdate { get; set; }
    }
}
