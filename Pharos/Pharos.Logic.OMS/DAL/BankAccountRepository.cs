﻿using Pharos.Logic.OMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharos.Logic.OMS.Entity.View;
using System.Data;
using System.Data.SqlClient;
using Pharos.Logic.OMS.IDAL;

namespace Pharos.Logic.OMS.DAL
{
    /// <summary>
    /// DAL商家结算账户
    /// </summary>
    public class BankAccountRepository : BaseRepository<BankAccount>, IBankAccountRepository
    {
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="CurrentPage">当前页,从1开始,不是0</param>
        /// <param name="PageSize">每页显示多少条数据</param>
        /// <param name="strw">where条件</param>
        /// <param name="Count">总数</param>
        /// <returns></returns>
        public List<ViewBankAccount> getPageList(int CurrentPage, int PageSize, string strw, out int Count)
        {
            string Table = "";
            Table = Table + "BankAccount b ";
            Table = Table + "left join PayLicense p on p.LicenseId=b.LicenseId ";
            Table = Table + "left join SysUser u on u.UserId=p.DesigneeId ";
            Table = Table + "left join SysUser u2 on u2.UserId=b.CreateUID ";
            Table = Table + "left join SysUser u3 on u3.UserId=b.AuditUID ";
            Table = Table + "left join Traders t on t.CID=b.CID ";

            string Fields = "";
            Fields = Fields + "b.Id,u.FullName,b.State,b.CID,t.FullTitle as TradersFullTitle,b.AccountType,b.AccountNumber,b.AccountName,b.BankName,b.LinkMan,b.Phone, ";
            Fields = Fields + "p.AgentsId,t.Status as TradersStatus,p.SourceType,b.CreateDT,u2.FullName as CreatePerson,b.AuditDT,u3.FullName as AuditPerson ";

            string Where = "1=1 ";
            if (strw != "")
            {
                Where = Where + strw;
            }

            string OrderBy = "b.Id desc ";

            return CommonDal.getPageList<ViewBankAccount>(Table, Fields, Where, OrderBy, CurrentPage, PageSize, 0, out Count);
        }
    }
}
