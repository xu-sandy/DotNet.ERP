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
    /// 商家门店支付通道DAL
    /// </summary>
    public class TradersPaySecretKeyRepository : BaseRepository<TradersPaySecretKey>, ITradersPaySecretKeyRepository
    {
        /// <summary>
        /// 获取CID
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<PayLicense> getListCID(string keyword,string strW = "")
        {
            using (EFDbContext db = new EFDbContext())
            {
                var sql = "select top 30 * from PayLicense where CID like " + keyword + strW + " order by CID";
                List<PayLicense> list = db.Database.SqlQuery<PayLicense>(sql).ToList();
                return list;
            }
        }

        /// <summary>
        /// 获取支付方式
        /// </summary>
        /// <param name="ChannelNo"></param>
        /// <returns></returns>
        public List<ViewPayChannelDetail> GetPayManner(int ChannelNo)
        {
            using (EFDbContext db = new EFDbContext())
            {
                var sql = "select top 100 ChannelDetailId,ChannelPayMode from PayChannelDetails where IsDeleted=0 and ChannelNo=" + ChannelNo;
                List<ViewPayChannelDetail> list = db.Database.SqlQuery<ViewPayChannelDetail>(sql).ToList();
                return list;
            }
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="CurrentPage">当前页,从1开始,不是0</param>
        /// <param name="PageSize">每页显示多少条数据</param>
        /// <param name="strw">where条件</param>
        /// <param name="Count">总数</param>
        /// <returns></returns>
        public List<ViewTradersPaySecretKey> getPageList(int CurrentPage, int PageSize, string strw, out int Count, string OrderBy)
        {
            string Table = "";
            Table = Table + "TradersPaySecretKey  p ";
            Table = Table + "left join TradersPayChannel p2 on p2.TPaySecrectId=p.TPaySecrectId ";
            Table = Table + "left join SysUser u on u.UserId=p.AssignUID ";
            Table = Table + "left join SysUser u2 on u2.UserId=p.CreateUID ";
            Table = Table + "left join SysUser u3 on u3.UserId=p.AuditUID ";
            Table = Table + "left join Traders t on t.CID=p.CID ";
            Table = Table + "left join PayChannelManages c on c.ChannelNo=p.ChannelNo ";

            string Fields = "";
            Fields = Fields + "p.Id,p.TPaySecrectId,u.FullName as Assign,p.State,p.CID,t.FullTitle as TradersFullTitle,p.SecretKey,p.MchId3,p.SecretKey3,c.ChannelCode,p2.ChannelPayMode,p2.PayNotifyUrl,p2.RfdNotifyUrl,p.CreateDT, ";
            Fields = Fields + "u2.FullName as CreateFullName,p.AuditDT,u3.FullName as AuditFullName ";

            string Where = "1=1 ";
            if (strw != "")
            {
                Where = Where + strw;
            }

            if (string.IsNullOrEmpty(OrderBy))
            {
                OrderBy = "p.CreateDT desc ";
            }
            
            return CommonDal.getPageList<ViewTradersPaySecretKey>(Table, Fields, Where, OrderBy, CurrentPage, PageSize, 0, out Count);
        }
    }
}
