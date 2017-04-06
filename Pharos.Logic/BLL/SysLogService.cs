using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Utility.Helpers;
using Pharos.Sys.Entity;
namespace Pharos.Logic.BLL
{
    public class SysLogService : BaseService<SysLog>
    {

        public static object GetLogs(out int recordCount,string keyword = "", int type = 0)
        {
            var result = (from e in SysLogService.CurrentRepository._context.Set<SysLog>()
                          join a in SysLogService.CurrentRepository._context.Set<SysUserInfo>() on e.UId equals a.UID
                          where (a.LoginName.Contains(keyword) || e.ClientIP.Contains(keyword) || string.IsNullOrEmpty(keyword)) && (e.Type == type || type == 0)
                          select new { e.Id,e.CreateDT,e.ClientIP,e.ServerName,e.Summary,e.Type,e.UId,a.LoginName });
            recordCount = result.Count();
            return result.ToPageList().Select(o=>new{
                ClientIP=o.ClientIP,
                CreateDT=o.CreateDT,
                Id=o.Id,
                ServerName=o.ServerName,
                TypeTitle=Enum.GetName(typeof(Sys.LogType),o.Type),
                UId=o.UId,
                o.LoginName,
                Summary = Summary(System.Web.HttpUtility.HtmlEncode(o.Summary))
            }).ToList();
        }
        static string Summary(string summary)
        {
            if (summary.IsNullOrEmpty() || summary.Length <= 180) return summary;
            return "<div title='"+summary+"'>" + summary.Substring(0, 180) + "...</div>";
        }
    }
}
