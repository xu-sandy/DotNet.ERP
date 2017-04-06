using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using Pharos.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharos.CRM.Retailing.Controllers
{
    /// <summary>
    /// 售后回访控制器
    /// </summary>
    public class FeedbackController : BaseController
    {
        public ActionResult FollowUp(int id)
        {
            var member = MembersService.FindById(id);
            if (member == null) return Content("查无此会员！若连续发生此错误请联系技术管理员。");
            ViewBag.SourceStore = WarehouseService.IsExist(a => a.StoreId == member.StoreId) ? WarehouseService.Find(a => a.StoreId == member.StoreId).Title : "";
            return View(member);
        }


        [HttpPost]
        public ActionResult GetFollowUpData()
        {
            int count;
            var list = FeedbackService.FindPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }

        [HttpPost]
        public ActionResult SaveFeedback(string memberId, string content)
        {
            return Content(FeedbackService.Add(new Feedback()
            {
                Content = content,
                CreateDT = DateTime.Now,
                CreateUID = Sys.CurrentUser.UID,
                FeedbackId = Sys.SysCommonRules.GUID,
                MemberId = memberId
            }).ToJson());
        }
    }
}
