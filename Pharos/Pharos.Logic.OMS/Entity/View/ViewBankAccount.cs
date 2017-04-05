﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Entity.View
{
    public class ViewBankAccount
    {
        /// <summary>
        /// 记录ID
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// 指派人
        /// </summary>
        public string FullName
        {
            get;
            set;
        }

        /// <summary>
        /// 账户状态
        /// </summary>
        public int State
        {
            get;
            set;
        }

        /// <summary>
        /// 商户号
        /// </summary>
        public int CID
        {
            get;
            set;
        }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string TradersFullTitle
        {
            get;
            set;
        }

        /// <summary>
        /// 结算账户类型
        /// </summary>
        public short AccountType
        {
            get;
            set;
        }

        /// <summary>
        /// 结算卡号
        /// </summary>
        public string AccountNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 账号名称
        /// </summary>
        public string AccountName
        {
            get;
            set;
        }

        /// <summary>
        /// 开户银行
        /// </summary>
        public string BankName
        {
            get;
            set;
        }

        /// <summary>
        /// 财务联系人
        /// </summary>
        public string LinkMan
        {
            get;
            set;
        }

        /// <summary>
        /// 财务联系电话
        /// </summary>
        public string Phone
        {
            get;
            set;
        }

        /// <summary>
        /// 代理商编号
        /// </summary>
        public int? AgentsId
        {
            get;
            set;
        }

        /// <summary>
        /// 商户状态
        /// </summary>
        public short TradersStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 所属体系
        /// </summary>
        public short SourceType
        {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDT
        {
            get;
            set;
        }

        /// <summary>
        /// 申请人
        /// </summary>
        public string CreatePerson
        {
            get;
            set;
        }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditDT
        {
            get;
            set;
        }

        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditPerson
        {
            get;
            set;
        }
    }
}
