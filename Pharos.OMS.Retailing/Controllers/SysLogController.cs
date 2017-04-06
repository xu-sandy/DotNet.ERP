using Pharos.Logic.OMS.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharos.OMS.Retailing.Controllers
{
    public class SysLogController : BaseController
    {
        //
        // GET: /SysLog/
        [Ninject.Inject]
        LogService LogService { get; set; }
        [Ninject.Inject]
        SysUserService UserService { get; set; }
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 日志管理-页面数据加载
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public ActionResult GetLogs(int page = 1, int rows = 30)
        {
            var count = 0;
            var list = LogService.GetPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }
        /// <summary>
        /// 日志管理-弹窗查看日志详情
        /// </summary>
        /// <param name="id">日志ID</param>
        /// <returns></returns>
        public ActionResult LogView(int id)
        {
            var model = LogService.GetOne(id);
            var user = UserService.GetOneByUID(model.UId);
            ViewBag.UserLoginName = user == null ? model.UId : user.LoginName;
            return View(model);
        }
        /// <summary>
        /// 日志管理-删除所选日志
        /// </summary>
        /// <param name="Ids">日志ID数组</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteLogs(int[] Ids)
        {
            var re = LogService.DeleteRange(Ids);
            return Json(new { successed = re });
        }
        /// <summary>
        /// 日志管理-删除全部日志
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteAll()
        {
            var re = LogService.DeleteAll();
            return Json(new { successed = re });
        }
    }
}
