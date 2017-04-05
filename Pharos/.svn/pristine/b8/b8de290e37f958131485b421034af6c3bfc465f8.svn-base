using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using Pharos.Logic.OMS;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.BLL;
using System.Linq;
namespace Pharos.OMS.Retailing.Controllers
{
    public class UserController : BaseController
    {
        [Ninject.Inject]
        private UserService UserService { get; set; }
        [Ninject.Inject]
        private SysUserService SysUserService { get; set; }
        [Ninject.Inject]
        private MenuService MenuService { get; set; }
        [Ninject.Inject]
        private LogEngine LogService { get; set; }
        //public UserController(IOMSSysUserInfoBLL userBll)
        //{
        //    _userBLL = userBll;
        //}
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetUsers(int page = 1, int rows = 30)
        {
            int count=0;
            var entities = UserService.GetPageList(Request.Params, out count);
            return ToDataGrid(entities, count);
        }
        /// <summary>
        /// 用户管理-新增或编辑用户表单-页面加载
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UserSave(int id = 0)
        {
            var model=id==0?new SysUserInfo(): UserService.GetOne(id);
            ViewBag.sysUserState = EnumToSelect(typeof(SysUserState));
            ViewBag.menus = ListToSelect(MenuService.GetChildList().Select(o => new SelectListItem() { Value=o.MenuId.ToString(),Text=o.Title}));
            return View(model.IsNullThrow());
        }
        /// <summary>
        /// 用户管理-新增或编辑用户表单-保存用户方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserSave(SysUserInfo model)
        {
            model.Limits = Request["Limits"];
            var result = UserService.SaveOrUpdate(model);
            return new OpActionResult(result);
        }
        [HttpPost]
        public ActionResult DeleteUser(int[] ids)
        {
            var op = UserService.Deletes(ids);
            return new OpActionResult(op);
        }
        [HttpPost]
        public ActionResult SetState(string ids, short state)
        {
            return new OpActionResult(UserService.SetState(ids,state));
        }
        public ActionResult Login()
        {
            //if (CurrentUser.IsLogin)
            //{
            //    //已登录，则直接进入主界面
            //    return Redirect(Url.Action("Index", "Home"));
            //}
            var user = new UserLogin();
            if (Cookies.IsExist("remuc"))
            {
                user.UserName = Cookies.Get("remuc", "_uname");
                user.UserPwd = Cookies.Get("remuc", "_pwd");
                user.RememberMe = true;
            }
            return View(user);

        }

        [HttpPost]
        public ActionResult Login(UserLogin user)
        {
            if (!ModelState.IsValid) return View(user);
            var obj = SysUserService.Login(user.UserName, user.UserPwd);
            if (obj == null)
            {
                ViewBag.msg = "帐户或密码输入不正确!";
                return View(user);
            }
            new CurrentUser().Login(obj, user.RememberMe);
            LogService.WriteLogin(string.Format("用户（{0}，{1}）成功登录系统！", obj.LoginName, obj.FullName), LogModule.其他);
            return Redirect(Url.Action("Index", "Home"));
        }
        public ActionResult Logout()
        {
            CurrentUser.Exit();
            return RedirectToAction("Login");
        }
        #region 个人信息
        public ActionResult UserInfo()
        {
            var model = UserService.GetOne(CurrentUser.ID);
            return View(model);
        }
        [HttpPost]
        public ActionResult UserInfo(int Id, string LoginPwd)
        {
            var model = UserService.GetOne(Id);
            model.LoginPwd = LoginPwd;
            var result = UserService.SaveOrUpdate(model);
            return Content(result.ToJson());
        }
        #endregion

        public ActionResult ChangePassword()
        {
            var model = UserService.GetOneByUID(CurrentUser.UID);
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangePassword(string LoginPwd)
        {
            OpResult result = new OpResult();
            if (LoginPwd.Trim().IsNullOrEmpty())
            {
                result.Successed = true;
            }
            else
            {
                SysUserInfo u = UserService.GetOneByUID(CurrentUser.UID);
                u.LoginPwd = LoginPwd;
                result = UserService.SaveOrUpdate(u);
            }
            return new OpActionResult(result);
        }

    }
    public class UserLogin
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名", Description = "4-20个字符。")]
        [Required(ErrorMessage = "×")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "×")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码", Description = "6-20个字符。")]
        [Required(ErrorMessage = "×")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "×")]
        [DataType(DataType.Password)]
        public string UserPwd { get; set; }

        public bool RememberMe { get; set; }

        public string StoreId { get; set; }
    }
}
