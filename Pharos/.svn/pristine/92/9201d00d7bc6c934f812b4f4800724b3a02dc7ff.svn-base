using System.Linq;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
using Pharos.Utility;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.BLL;

namespace QCT.Pay.Admin.Controllers
{
    public class BusinessController : BaseController
    {
        //
        // GET: /Brand/
        [Ninject.Inject]
        BusinessService BusinessService { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FindPageList(string title, int? classfyId)
        {
            var count = 0;
            var list = BusinessService.GetPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }
        [HttpPost]
        public ActionResult Delete(int[] ids)
        {
            var op = BusinessService.Deletes(ids);
            return new OpActionResult(op);
        }
        //新增品牌
        public ActionResult Save(int? id)
        {
            var obj = new Business();
            if (id.HasValue)
            {
                obj = BusinessService.GetOne(id.Value);
            }
            return View(obj.IsNullThrow());
        }
        [HttpPost]
        public ActionResult Save(Business obj)
        {
            var op = BusinessService.SaveOrUpdate(obj);
            return new OpActionResult(op);
        }
        [HttpPost]
        public ActionResult SetState(string ids, short state)
        {
            var op = BusinessService.SetState(ids, state);
            return new JsonNetResult(op);
        }
    }
}
