﻿using System.Collections.Generic;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
using Pharos.Logic.OMS.BLL;
using Pharos.Logic.OMS.Entity;
using System.Linq;
using Pharos.Logic.OMS;
namespace Pharos.OMS.Retailing.Controllers
{
    public class CompanyAuthorController : BaseController
    {
        [Ninject.Inject]
        CompAuthorService CompAuthorService { get; set; }
        [Ninject.Inject]
        DictionaryService DictionaryService { get; set; }
        [Ninject.Inject]
        BusinessService BusinessService { get; set; }
        [Ninject.Inject]
        SysUserService UserService { get; set; }
        [Ninject.Inject]
        ProductPublishVerService ProductPublishVerService { get; set; }
        [Ninject.Inject]
        ProductVerService ProductVerService { get; set; }
        //
        // GET: /Authorization/
        [SysPermissionValidate]
        public ActionResult Index()
        {
            ViewBag.users = ListToSelect(UserService.GetList().Select(o => new SelectListItem() { Text = o.FullName, Value = o.UserId }), emptyTitle: "全部");
            ViewBag.states = EnumToSelect(typeof(CompanyAuthorizeState), emptyTitle: "全部");
            ViewBag.products = ListToSelect(ProductVerService.GetList(1).Select(o => new SelectListItem() { Text = "（" + o.ProductId + "）" + o.SysName, Value = o.ProductId.ToString() }), emptyTitle: "全部");
            return View();
        }
        [HttpPost]
        public ActionResult FindPageList()
        {
            int count = 0;
            var list = CompAuthorService.GetPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }
        public ActionResult Save(int? id)
        {
            var obj = new CompanyAuthorize() { Source=1,Way=1,MemberShared="Y",EffectiveDT=System.DateTime.Now.ToString("yyyy-MM-dd")};
            if (id.HasValue)
            {
                obj = CompAuthorService.GetOne(id.Value);
            }
            ViewBag.BusinessModes = ListToSelect(DictionaryService.GetChildList(221,false).Select(o => new SelectListItem() { Text = o.Title, Value = o.DicSN.ToString() }));
            ViewBag.OpenScopeIds = ListToSelect(BusinessService.GetList(false).Select(o => new SelectListItem() { Text = o.Title, Value = o.ById }));
            ViewBag.users = ListToSelect(UserService.GetList(false).Select(o => new SelectListItem() {Text=o.FullName,Value=o.UserId }), emptyTitle: "请选择");
            ViewBag.states = EnumToSelect(typeof(CompanyAuthorizeState));
            ViewBag.products = ListToSelect(ProductPublishVerService.GetProductVerEnables().Select(o => new SelectListItem() { Text = "（" + o.ProductId + "）" + o.SysName, Value = o.ProductId.ToString() }), emptyTitle: "请选择");
            return View(obj.IsNullThrow());
        }
        public ActionResult Add(int id)
        {
            ViewBag.users = ListToSelect(UserService.GetList(false).Select(o => new SelectListItem() { Text = o.FullName, Value = o.UserId }), emptyTitle: "请选择");
            var obj= CompAuthorService.GetOne(id);
            obj.IsNullThrow();
            obj.EffectiveDT = System.DateTime.Now.ToString("yyyy-MM-dd");
            return View(obj);
        }
        [HttpPost]
        public ActionResult Save(CompanyAuthorize obj)
        {
            obj.OpenScopeId = Request["OpenScopeId"];
            var op = CompAuthorService.SaveOrUpdate(obj);
            return new OpActionResult(op);
        }
        [HttpPost]
        public ActionResult Add(CompanyAuthorize obj)
        {
            var op = CompAuthorService.Add(obj);
            return new OpActionResult(op);
        }
        [HttpPost]
        public ActionResult Delete(int[] ids)
        {
            var op = CompAuthorService.Deletes(ids);
            return Content(op.ToJson());
        }
        [HttpPost]
        public ActionResult SetState(string ids,short state)
        {
            var op = CompAuthorService.SetState(ids, state);
            return Content(op.ToJson());
        }
        public ActionResult GenerateSerial(string codes)
        {
            var dicts = CompAuthorService.GenerateSerial(codes);
            Session["GenerateSerials"] = dicts;
            return View(dicts);
        }
        [HttpPost]
        public ActionResult GenerateSerial()
        {
            var dicts = Session["GenerateSerials"] as Dictionary<string, string>;
            return Content(Pharos.Utility.OpResult.Success().ToJson());
        }
        public string GetCompany(string companyId)
        {
            var source=new CompanyAuthorize();
            //int compid = 0;
            //if (int.TryParse(companyId, out compid))
            //    source = CompAuthorService.GetOneByCID(compid);
            //else
            //{
            //    var auth = new Sys.SysAuthorize();
            //    source = auth.AnalysisSN(companyId);
            //    if(source!=null)
            //    {
            //        //if (!auth.ValidateCompany(source, CompAuthorService.GetOneByCID(source.Code.GetValueOrDefault())))
            //            source.Code=0;
            //    }
            //}
            return source==null?"":source.ToJson();
        }
    }
}
