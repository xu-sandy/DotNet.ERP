using Pharos.Logic.OMS;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.EntityExtend;
using Pharos.Logic.OMS.Entity.View;
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
    public class AgentsUsersController : BaseController
    {
        //代理商账号
        [Ninject.Inject]
        AgentsUsersService agentsUsersService { get; set; }
        
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="AgentsId"></param>
        /// <returns></returns>
        public ActionResult Verification(string AgentsId)
        {
            var op = agentsUsersService.Verification(AgentsId);
            return new OpActionResult(op);
        }

        public ActionResult Save(int? id)
        {
            var obj = new AgentsUsers
            {

            };
            if (id.HasValue)
            {
                obj = agentsUsersService.GetOne(id.Value);
                obj.LoginPwd = "";
            }
            return View(obj.IsNullThrow());
        }

        [HttpPost]
        public ActionResult Save(AgentsUsers agentsUsers)
        {
            var op = agentsUsersService.SaveOrUpdate(agentsUsers);
            return new OpActionResult(op);
        }

        public ActionResult FindPageList()
        {
            var count = 0;
            var list = agentsUsersService.GetPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }

        /// <summary>
        /// 将所选项设为
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult upStatus(string ids, int Status)
        {
            return new JsonNetResult(agentsUsersService.upStatus(ids, Status));
        }
    }
}
