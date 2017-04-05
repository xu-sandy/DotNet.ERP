using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.BLL;
using Pharos.Logic.DAL;
using Pharos.Sys.Entity;

namespace Pharos.Logic.ApiData.Mobile.Services
{
    public class UserInfoService : BaseGeneralService<SysUserInfo, EFDbContext>
    {
        public static SysUserInfo Login(string account, string password)
        {
            var user = Find(o => o.LoginName == account && o.Status == 1);
            if (user == null)
            {
                throw new Exceptions.LoginExecption("账号错误！");
            }
            else if (user.LoginPwd != password)
            {
                throw new Exceptions.LoginExecption("密码错误！");
            }
            return user;
        }
    }
}
