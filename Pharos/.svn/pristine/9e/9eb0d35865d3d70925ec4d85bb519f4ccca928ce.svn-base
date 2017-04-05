using Pharos.Logic.LocalEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.LocalServices
{
    public class ApiLibraryLocalService : BaseLocalService<ApiLibrary>
    {
        /// <summary>
        /// 获取支付方式
        /// </summary>
        /// <param name="ids">支付方式多个ApiCode</param>
        /// <returns></returns>
        public static List<ApiLibrary> GetPayWays(IEnumerable<int> ids)
        {
            if (ids != null)
            {
                return CurrentRepository.FindList(o => ids.Contains(o.ApiCode)).ToList();
            }
            return null;
        }

        public static int GetPayCode(string title) 
        {
            var entity = CurrentRepository.Find(o => o.Title == title);
            if (entity != null) 
            {
                return entity.ApiCode;
            }
            return 0;
        }


        public static string GetApiUrl(int apiCode) 
        {
            var entity = CurrentRepository.Find(o => o.ApiCode == apiCode);
            if (entity != null)
            {
                return entity.ApiUrl;
            }
            return "";
        }
    }
}
