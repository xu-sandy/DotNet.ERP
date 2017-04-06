using Pharos.Logic.OMS;
using Pharos.Logic.OMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
using Pharos.Utility;
using Pharos.Logic.OMS.BLL;

namespace Pharos.OMS.Retailing.Controllers
{
    public class TraderTypeController : BaseController
    {
        [Ninject.Inject]
        TraderTypeService TraderTypeService { get; set; }

        [SysPermissionValidate(71)]
        public ActionResult Index()
        {
            ViewBag.list = TraderTypeService.getList();
            return View();
        }

        //添加
        [HttpPost]
        public ActionResult Index(TraderType t)
        {
            var op = TraderTypeService.SaveOrUpdate(t);
            return new OpActionResult(op);
        }

        [HttpPost]
        public ActionResult Delete(string TraderTypeId)
        {
            return new JsonNetResult(TraderTypeService.Delete(TraderTypeId));
        }

    }
}
