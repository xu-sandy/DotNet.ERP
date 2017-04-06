using Pharos.Logic.OMS.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharos.OMS.Retailing.Controllers
{
    public class TradersController : BaseController
    {
        [Ninject.Inject]
        TradersService TradersService { get; set; }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetTraderInput(string searchName)
        {
            var list = TradersService.GetTraderInput(searchName);
            return ToDataGrid(list, 0);
        }
    }
}
