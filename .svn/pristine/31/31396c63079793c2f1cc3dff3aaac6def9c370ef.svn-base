using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
using Pharos.Logic;
using Pharos.Logic.BLL;
namespace Pharos.CRM.Retailing.Controllers
{
    /// <summary>
    /// 门店收货
    /// </summary>
    public class ShouhuoController : BaseController
    {
        //
        // GET: /Shouhuo/

        public ActionResult Index()
        {
            ViewBag.states = EnumToSelect(typeof(OrderState), emptyTitle: "请选择");
            return View();
        }
        [HttpPost]
        public ActionResult FindPageList()
        {
            int count = 0;
            var list = ShouHuoService.FindPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }
        public ActionResult Receive(string id)
        {
            var obj = ShouHuoService.GetObj(id);
            return View(obj);
        }
        [HttpPost]
        public ActionResult Receive(string id, decimal receivedNum)
        {
            var op = ShouHuoService.Update(id, receivedNum);
            return Content(op.ToJson());
        }
        [HttpPost]
        public ActionResult SetState(string ids)
        {
            var op = ShouHuoService.Update(ids,null);
            return Content(op.ToJson());
        }
        public ActionResult TuiHuang(string id, string barcode)
        {
            ViewBag.reasons =ListToSelect(SysDataDictService.GetReasonTitle().Select(o=>new SelectListItem(){Text=o.Title,Value=o.DicSN.ToString()}));
            var order= OrderReturnBLL.Find(o => o.DistributionId == id && o.Barcode == barcode);
            object obj = null;
            if(order!=null)
                obj= OrderReturnBLL.FindDetailById(order.Id);
            else
                obj= ShouHuoService.GetObj(id);
            return View(obj);
        }
        [HttpPost]
        public ActionResult TuiHuang(Pharos.Logic.Entity.OrderReturns obj)
        {
            var op = ShouHuoService.SaveTuiHuang(obj, Request["ReceiveNum"]);
            return Content(op.ToJson());
        }
        [HttpPost]
        public ActionResult LoadGiftList(int id, string orderId, string barcode)
        {
            var list = ShouHuoService.LoadGiftList(id,orderId, barcode);
            return ToDataGrid(list, 0);
        }
    }
}
