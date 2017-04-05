using Pharos.Sys.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public abstract class BaseSupplier
    {
        /// <summary>
        /// 供应商ID
        /// [主键：√]
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [OperationLog("ID", false)]
        public string Id { get; set; }

        /// <summary>
        /// 供应商分类（-1:未知）（来自数据字典表）
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((-1))]
        /// </summary>
        [OperationLog("供应商分类", true)]
        public int ClassifyId { get; set; }

        public int CompanyId { get; set; }
        /// <summary>
        /// 商家简称
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        [OperationLog("商家简称", false)]
        public string Title { get; set; }

        /// <summary>
        /// 商家简拼
        /// [长度：20]
        /// </summary>
        [OperationLog("商家简拼", false)]
        public string Jianpin { get; set; }

        /// <summary>
        /// 商家全称
        /// [长度：100]
        /// [不允许为空]
        /// </summary>
        [OperationLog("商家全称", false)]
        public string FullTitle { get; set; }

        /// <summary>
        /// 联系人
        /// [长度：20]
        /// [不允许为空]
        /// </summary>
        [OperationLog("联系人", false)]
        public string Linkman { get; set; }

        /// <summary>
        /// 手机
        /// [长度：11]
        /// </summary>
        [OperationLog("手机", false)]
        public string MobilePhone { get; set; }

        /// <summary>
        /// 电话
        /// [长度：20]
        /// </summary>
        [OperationLog("电话", false)]
        public string Tel { get; set; }

        /// <summary>
        /// E-mail
        /// [长度：100]
        /// </summary>
        [OperationLog("E-mail", false)]
        public string Email { get; set; }

        /// <summary>
        /// 地址
        /// [长度：100]
        /// </summary>
        [OperationLog("地址", false)]
        public string Address { get; set; }

        /// <summary>
        /// 指派人（UID）
        /// [长度：40]
        /// [不允许为空]
        /// [默认值：((-1))]
        /// </summary>
        [OperationLog("指派人", false)]
        public string Designee { get; set; }

        /// <summary>
        /// 主账号
        /// [长度：100]
        /// </summary>
        [OperationLog("主账号", false)]
        public string MasterAccount { get; set; }

        /// <summary>
        /// 密码
        /// [长度：50]
        /// </summary>
        public string MasterPwd { get; set; }

        /// <summary>
        /// 账号状态（0:关闭、1:可用）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [OperationLog("账号状态", "0:关闭", "1:可用")]
        public short MasterState { get; set; }

        public DateTime? CreateDT { get; set; }

        /// <summary>
        /// 商家类型（1：供应商、2：批发商）
        /// </summary>
        [OperationLog("商家类型", "1:供应商", "2:批发商")]
        public short BusinessType { get; set; }
        /// <summary>
        /// 手势密码
        /// </summary>
        public string Handsign { get; set; }
        /// <summary>
        /// 个性签名
        /// </summary>
        [OperationLog("个性签名", false)]
        public string Signature { get; set; }
    }
}
