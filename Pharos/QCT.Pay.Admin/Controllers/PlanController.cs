﻿using Pharos.Logic.OMS;
using Pharos.Logic.OMS.BLL;
using Pharos.Logic.OMS.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utility = Pharos.Utility;

namespace QCT.Pay.Admin.Controllers
{
    public class PlanController : BaseController
    {
        [Ninject.Inject]
        PlanService PlanService { get; set; }
        [Ninject.Inject]
        DictionaryService DictionaryService { get; set; }
        [Ninject.Inject]
        UserService UserService { get; set; }

        [Ninject.Inject]
        TradersService tradersService { get; set; }
        #region 所有计划
        //
        // GET: /Plan/

        public ActionResult Index()
        {
            ViewBag.users = ListToSelect(UserService.GetList(false).Select(o => new SelectListItem() { Value = o.UserId, Text = o.FullName }),emptyTitle:"全部");
            var dicts = DictionaryService.GetChildList(new List<int>() { 369, 370 });
            ViewBag.status = ListToSelect(dicts.Where(o => o.DicPSN == 370).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "全部");
            ViewBag.types = ListToSelect(dicts.Where(o => o.DicPSN == 369).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "全部");
            return View();
        }
        [HttpPost]
        public ActionResult IndexPageList()
        {
            var count = 0;
            var nvl = new System.Collections.Specialized.NameValueCollection() { Request.Params};
            nvl.Add("all", "1");
            var list = PlanService.IndexPageList(nvl, out count);
            return ToDataGrid(list, count);
        }
        #endregion
        #region 我的计划
        public ActionResult MyIndex()
        {
            var dicts = DictionaryService.GetChildList(new List<int>() { 369, 370 });
            ViewBag.status = ListToSelect(dicts.Where(o => o.DicPSN == 370).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "全部");
            ViewBag.statuuses = ListToSelect(dicts.Where(o => o.Status && o.DicPSN == 370).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "将所选项设为指定状态");
            ViewBag.types = ListToSelect(dicts.Where(o => o.DicPSN == 369).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "全部");
            return View();
        }
        [HttpPost]
        public ActionResult MyIndexPageList()
        {
            var count = 0;
            var list = PlanService.IndexPageList(Request.Params, out count);
            return ToDataGrid(list,count);
        }

        public ActionResult SavePlan(string id)
        {
            string LeaderFullName = "";
            ViewBag.types=ListToSelect( DictionaryService.GetChildList(369,false).Select(o=>new SelectListItem(){Value=o.DicSN.ToString(),Text=o.Title}));
            ViewBag.users=ListToSelect(UserService.GetList(false).Select(o=>new SelectListItem(){Value=o.UserId,Text=o.FullName}));
            var obj = new Pharos.Logic.OMS.Entity.Plans() { Type = 372,AssignerUID=CurrentUser.UID };
            if (!string.IsNullOrWhiteSpace(id))
            {
                obj = PlanService.GetOne(id);
                LeaderFullName = tradersService.getLeaderFullName(obj.LeaderUID);
            }   
            Attachments = null;
            ViewBag.LeaderFullName = LeaderFullName;
            return View(obj);
        }
        [HttpPost]
        public ActionResult SavePlan(Plans plan)
        {
            plan.Attachments = Attachments;
            plan.LeaderUID = Request["LeaderUID"];
            var op = PlanService.SaveOrUpdate(plan);
            if (op.Successed) Attachments = null;
            return new OpActionResult(op);
        }
        [HttpPost]
        public ActionResult DeletePlan(string[] ids)
        {
            var op = PlanService.DeletePlan(ids);
            return new OpActionResult(op);
        }
        public ActionResult LoadFileList(string id)
        {
            var obj = PlanService.GetOne(id);
            if(Attachments!=null)
                obj.Attachments.AddRange(Attachments);
            return ToDataGrid(obj.Attachments, 0);
        }
        [HttpPost]
        public ActionResult FileUpload()
        {
            Attachments =Attachments?? new List<Attachment>();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[0];
                if (file.ContentLength <= 0) continue;
                if (file.ContentLength > 10 * 1024 * 1024)
                {
                    return new OpActionResult(Utility.OpResult.Fail("文件超过10M"));
                }
                var ext = Path.GetExtension(file.FileName)??"";
                var sm = file.InputStream;
                var buffer = new byte[sm.Length];
                sm.Read(buffer, 0, buffer.Length);
                var obj = new Attachment()
                {
                    ExtName = ext.TrimStart('.'),
                    FileSize = file.ContentLength,
                    Name = Path.GetFileName(file.FileName),
                    NewName=Guid.NewGuid().ToString(),
                    TableName = "Plans",
                    Bytes=buffer
                };
                Attachments.Add(obj);
            }
            return new OpActionResult(Utility.OpResult.Success());
        }
        public ActionResult RemoveFile(string id, string name)
        {
            if(string.IsNullOrWhiteSpace( name))
            {
                return new OpActionResult(Utility.OpResult.Fail("传参数为空！"));
            }
            if(Attachments!=null)
            {
                var obj= Attachments.FirstOrDefault(o => o.NewName == name);
                if (obj != null)
                {
                    Attachments.Remove(obj);
                    return new OpActionResult(Utility.OpResult.Success());
                }
            }
            var op = PlanService.RemoveFile(id, name);
            return new OpActionResult(op);
        }
        public ActionResult Preview(string id,string name)
        {
            var op = PlanService.Preview(id, name);
            ViewBag.filename = op.Message;
            return View();
        }
        public ActionResult DownLoad(int id, string rootid)
        {
            var file = PlanService.DownLoad(id);
            if (!System.IO.File.Exists(file))
                return RedirectAlert("SavePlan", "文件不存在！", new { id = rootid });
            return File(file,Pharos.Utility.Helpers.FileHelper.GetContentType(Path.GetExtension(file)),Path.GetFileName(file));
        }
        [HttpPost]
        public ActionResult UpdateStatus(string ids, short status)
        {
            var op = PlanService.UpdateStatus(ids, status);
            return new OpActionResult(op);
        }
        List<Attachment> Attachments { get { return Session["attachs"] as List<Attachment>; } set { Session["attachs"] = value; } }

        public ActionResult getUserWhere()
        {
            var list = UserService.GetListWhere(Request.Params);
            return ToDataGrid(list, 0);
        }


        public string isLeaderFullName(string leaderUID)
        {
            string s = tradersService.getLeaderFullName(leaderUID);
            if (string.IsNullOrEmpty(s))
            {
                //不存在
                return "0";
            }
            else
            {
                //存在
                return "1";
            }
        }

        #endregion
    }
}
