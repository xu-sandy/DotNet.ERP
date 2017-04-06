using System;
using System.Web;
using System.Collections.Generic;
using Pharos.Utility;
using Pharos.Logic.OMS.Entity;
using System.Linq;
namespace Pharos.Logic.OMS
{
    /// <summary>
    /// 当前登录用户信息
    /// </summary>
    public class CurrentUser
    {
        /// <summary>
        /// 是否已登录
        /// </summary>
        public static bool IsLogin
        {
            get
            {
                return !string.IsNullOrEmpty(UID);
            }
        }

        /// <summary>
        /// 统一登录写入cookie
        /// </summary>
        /// <param name="user">Entity.SysUser 用户信息类</param>
        /// <param name="remember">记住用户和密码，默认false</param>
        public void Login(SysUser user, bool remember = false)
        {
            Dictionary<string, string> kv = new Dictionary<string, string>();

            kv.Add(key_uid, user.UserId);
            kv.Add(key_id, user.Id.ToString());
            kv.Add(key_uname, user.LoginName);
            kv.Add(key_fname, HttpUtility.UrlEncode(user.FullName));
            kv.Add(key_roleId, user.RoleIds);
            kv.Add(key_super, user.IsSuper.ToString());
            kv.Add(key_dept, user.DeptId.ToString());
            Cookies.Remove("remuc");
            if (remember)
            {
                Cookies.Set("remuc", kv, 30, true);//防止退出删除
            }
            Cookies.Set(uc, kv);
        }

        private const string uc = "uc";
        private const string key_pwd = "_pwd";

        /// <summary>
        /// 安全退出
        /// </summary>
        public static void Exit()
        {
            if (IsLogin)
            {
                HttpCookie hc = new HttpCookie(uc);
                hc.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(hc);
            }
        }

        public static string PWD
        {
            get
            {
                return Cookies.Get(uc, key_pwd);
            }
        }

        /// <summary>
        /// 用户UID
        /// </summary>
        public static string UID
        {
            get
            {
                return Cookies.Get(uc, key_uid);
            }
        }
        private const string key_uid = "_u";
        /// <summary>
        /// 用户UID
        /// </summary>
        public static bool IsSuper
        {
            get
            {
                var val= Cookies.Get(uc, key_super);
                var b = false;
                bool.TryParse(val,out b);
                return b;
            }
        }
        private const string key_super = "_sr";
        /// <summary>
        /// 部门ID
        /// </summary>
        public static int DeptId
        {
            get
            {
                var val = Cookies.Get(uc, key_dept);
                var b = 0;
                int.TryParse(val, out b);
                return b;
            }
        }
        private const string key_dept = "_dept";
        /// <summary>
        /// 用户UID
        /// </summary>
        public static int ID
        {
            get
            {
                var id= Cookies.Get(uc, key_id);
                if (string.IsNullOrWhiteSpace(id)) return 0;
                var i=0;
                int.TryParse(id, out i);
                return i;
            }
        }
        private const string key_id = "_id";

        /// <summary>
        /// 登录名称
        /// </summary>
        public static string UserName
        {
            get
            {
                return HttpUtility.UrlDecode(Cookies.Get(uc, key_uname));
            }
        }
        private const string key_uname = "_uname";

        /// <summary>
        /// 用户姓名
        /// </summary>
        public static string FullName
        {
            get
            {
                return HttpUtility.UrlDecode(Cookies.Get(uc, key_fname));
            }
        }
        private const string key_fname = "_fname";

        /// <summary>
        /// 角色ID
        /// </summary>
        public static string RoleIds
        {
            get
            {
                return Cookies.Get(uc, key_roleId);
            }
        }
        private const string key_roleId = "_roleId";


        /// <summary>
        /// 权限模块
        /// </summary>
        public static string Limits
        {
            get
            {
                return Cookies.Get(uc, key_limits);
            }
        }
        private const string key_limits = "_limits";
        static object objLock = new object();
        /// <summary>
        /// 判断用户是否有对应的访问权限
        /// </summary>
        /// <param name="limitId"></param>
        /// <returns></returns>
        public static bool HasPermiss(int limitId)
        {
            if (CurrentUser.IsSuper) return true;
            var result = true;
            lock (objLock)
            {
                var menus = DataCache.Get<List<Models.SysMenuLimitModel>>("limits");
                if (menus == null)
                {
                    var menuService = NinjectObject.GetFromMVC<Pharos.Logic.OMS.BLL.MenuService>();
                    menus = menuService.GetUserMenus(UID);
                    //DataCache.Set("limits", menus, 1);
                }
                if (HttpContext.Current != null && HttpContext.Current.Request.RawUrl != "/")
                {
                    result = menus.Any(o => o.URL != null && HttpContext.Current.Request.RawUrl.EndsWith(o.URL, StringComparison.CurrentCultureIgnoreCase));
                }
                if (limitId > 0)
                {
                    result = menus.Any(o => ("," + o.LimitIdStr + ",").Contains("," + limitId + ",") || o.LimitIdStr == "-1");
                }
            }
            return result;
        }
        public static List<string> GetAllCreateUIDByDeptId(int deptId)
        {
            lock (objLock)
            {
                var users = DataCache.Get<List<string>>("deptusers");
                if (users == null)
                {
                    var src = NinjectObject.GetFromMVC<Pharos.Logic.OMS.BLL.DepartMentService>();
                    users = src.GetAllCreateUIDByDeptId(deptId);
                    //DataCache.Set("deptusers", users, 1);
                }
                return users;
            }
        }
        public static List<int> GetDeptChildsByDeptId(int deptId)
        {
            lock (objLock)
            {
                var depts = DataCache.Get<List<int>>("deptchilds");
                if (depts == null)
                {
                    var src = NinjectObject.GetFromMVC<Pharos.Logic.OMS.BLL.DepartMentService>();
                    depts = src.GetDeptChildByDeptId(deptId);
                    //DataCache.Set("deptchilds", depts, 1);
                }
                return depts;
            }
        }
    }
}
