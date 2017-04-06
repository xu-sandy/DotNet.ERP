using Pharos.Sys.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharos.Utility.Helpers;
using System.Data.SqlClient;
using System.Data;
using Pharos.Utility;
using Pharos.Sys.DAL;

namespace Pharos.Sys.BLL
{
    public class SysLogBLL 
    {
        private SysLogDAL _dal = new SysLogDAL();

        /// <summary>
        /// 获得所有的日志列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetList(Paging paging, string key, int type,string start,string end)
        {
            return _dal.GetList(paging, key, type,start,end);
        }

        public SysLog GetLogById(int id)
        {
            return _dal.GetById(id);
        }

        public bool DeleteRange(int[] ids)
        {
            return _dal.DeleteRange(ids);
        }
        public bool DeleteAll()
        {
            return _dal.DeleteAll();
        }
    }
}
