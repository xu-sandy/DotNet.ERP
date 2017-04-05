using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Utility.Helpers;
using Pharos.Logic.Entity;
using System.Data.SqlClient;

namespace Pharos.Logic.DAL
{
    internal class PushDAL : BaseDAL
    {
        internal System.Data.DataTable GetPushResult(string pushId, string typeStr, System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            string keyword = nvl["keyword"];
            string sql = @"SELECT w.ID, m.RealName, m.MobilePhone, m.Weixin, m.Email, w.State, w.Channel FROM dbo.PushWithMember AS w
                        INNER JOIN dbo.Members AS m ON w.MemberId = m.MemberId
                        WHERE w.PushId = '" + pushId + "'";
            switch (typeStr)
            {
                case "mobile":
                    sql += " AND w.Channel = 153";
                    break;
                case "email":
                    sql += " AND w.Channel = 154";
                    break;
                case "weixin":
                    sql += " AND w.Channel NOT IN (153, 154)";
                    break;
                default:
                    break;
            }
            if (!keyword.IsNullOrEmpty())
                sql += " AND (m.RealName LIKE N'%" + keyword + "%' OR m.MobilePhone LIKE '%" + keyword + "%' OR m.Weixin LIKE '%" + keyword + "%' OR m.Email LIKE '%" + keyword + "%')";
            return base.ExceuteSqlForPage(sql, out recordCount);
        }

        internal int InsertPushWithMemberMappings(List<PushWithMember> pwms)
        {
            string sql = "INSERT INTO dbo.PushWithMember (PushId, MemberId, State, Channel) VALUES";
            foreach (var item in pwms)
            {
                sql += " ('" + item.PushId + "','" + item.MemberId + "'," + item.State + "," + item.Channel + "),";
            }
            sql = sql.Remove(sql.Length - 1, 1);
            return base._db.ExecuteNonQueryText(sql, null);
        }
    }
}
