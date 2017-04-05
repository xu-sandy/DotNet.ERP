using Pharos.Logic;
using Pharos.Logic.BLL;
using Pharos.Sys.Entity;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using System.Linq;
using System.Web.Mvc;
namespace Pharos.CRM.Retailing.Controllers
{
    public class VoucherTypeController : Controller
    {
        //
        // GET: /SupplierType/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Save()
        {
            var list = SysDataDictService.GetReceiptsCategories().Select(o => o.Title);
            return View(list);
        }
        [HttpPost]
        public ActionResult Save(string name)
        {
            var re = new OpResult();
            if (!SysDataDictService.IsExist(o => o.DicPSN == (int)DicType.单据类别 && o.Status && o.Title == name))
            {
                var max = SysDataDictService.GetMaxSN;
                re = SysDataDictService.Add(new SysDataDictionary()
                {
                    DicSN = max,
                    Title = name,
                    DicPSN = (int)DicType.单据类别,
                    Status = true,
                    Depth = 2
                });
            }
            else
                re.Message = "该分类名称已存在";
            return Content(re.ToJson());
        }
        [HttpPost]
        public ActionResult ListPartial()
        {
            var list = SysDataDictService.GetReceiptsCategories().Select(o => o.Title);
            return PartialView(list);
        }
        public ActionResult Delete(string id)
        {
            var obj = SysDataDictService.Find(o => o.DicPSN == (int)DicType.单据类别 && o.Status && o.Title == id);
            var re = SysDataDictService.Delete(obj);
            return Json(re);
        }
        public ActionResult GetList()
        {
            var list = SysDataDictService.GetReceiptsCategories();
            var json = list.Select(o => new DropdownItem(o.DicSN.ToString(), o.Title)).ToList();
            json.Insert(0, new DropdownItem("", "请选择"));
            return new JsonNetResult(json);
        }
    }
}
