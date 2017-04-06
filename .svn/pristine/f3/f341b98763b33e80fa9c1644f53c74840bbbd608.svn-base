using Pharos.Logic;
using Pharos.Logic.BLL;
using Pharos.Sys.Entity;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using System.Linq;
using System.Web.Mvc;
namespace Pharos.CRM.Retailing.Controllers
{
    public class SupplierTypeController : Controller
    {
        //
        // GET: /SupplierType/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Save()
        {
            var list = SysDataDictService.GetSupplierTypeTitles();
            return View(list);
        }
        [HttpPost]
        [SysPermissionValidate(Code = Sys.SysConstLimits.供应商_编辑分类)]
        public ActionResult Save(string name)
        {
            var re = new OpResult();
            if (!SysDataDictService.IsExist(o => o.DicPSN == (int)DicType.供应商分类 && o.Status && o.Title == name && o.CompanyId==CommonService.CompanyId))
            {
                var max = SysDataDictService.GetMaxSN;
                re= SysDataDictService.Add(new SysDataDictionary()
                {
                    DicSN =max,
                    Title=name,
                    DicPSN = (int)DicType.供应商分类,
                    Status=true,
                    Depth=2,
                    CompanyId=CommonService.CompanyId
                });
            }
            else
                re.Message = "该分类名称已存在";
            return Content(re.ToJson());
        }
        [HttpPost]
        public ActionResult ListPartial()
        {
            var list = SysDataDictService.GetSupplierTypeTitles();
            return PartialView(list);
        }
        public ActionResult Delete(string id)
        {
            var title = id.Substring(0, id.IndexOf("("));
            var obj = SysDataDictService.Find(o => o.DicPSN == (int)DicType.供应商分类 && o.Status && o.Title == title && o.CompanyId == CommonService.CompanyId);
            var re=new OpResult();
            if (SupplierService.IsExist(o => o.ClassifyId == obj.DicSN))
                re.Message = "存在供应商关联不允许删除!";
            else
                re= SysDataDictService.Delete(obj);
            return new JsonNetResult(re);
        }
        public ActionResult GetList()
        {
            var list = SysDataDictService.GetSupplierTypes();
            var json= list.Select(o => new DropdownItem(o.DicSN.ToString(), o.Title)).ToList();
            json.Insert(0, new DropdownItem("", "将所选项移到指定分类"));
            return Content(json.ToJson());
        }
    }
}
