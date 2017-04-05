using Pharos.Logic.DAL.Sqlite;
using Pharos.Logic.LocalEntity;
using System.Collections.Generic;
using System.Linq;

namespace Pharos.Logic.BLL.LocalServices
{
    public class SysStoreUserInfoLocalService : BaseLocalService<SysStoreUserInfo>
    {
        /// <summary>
        /// 用于POS端启动加速
        /// </summary>
        public static void InitForStart()
        {
            CurrentRepository.IsExist(o => true);
        }

        public static string IsSaleMan(string userCode, string storeId)
        {
            string operateAuth = string.Format("{0},{1}", storeId, 2);
            var result = CurrentRepository.Find(o => o.UserCode == userCode && o.OperateAuth.Contains(operateAuth));
            if (result != null)
            {
                return result.UID;
            }
            return string.Empty;
        }

    }


}
