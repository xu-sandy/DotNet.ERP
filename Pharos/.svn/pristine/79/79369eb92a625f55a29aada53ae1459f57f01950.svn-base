using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
namespace Pharos.CRM.Retailing.Controllers
{
    public class AttachController:BaseController
    {
        public void DownFile(int id)
        {
            var obj= Pharos.Logic.BLL.AttachService.FindById(id);
            obj.IsNullThrow();
            var fullName = System.IO.Path.Combine(Pharos.Sys.SysConstPool.GetRoot, obj.SaveUrl);
            Pharos.Utility.ExportExcel.ResponseFile(fullName, obj.Title);
            //return File(fullName, "application/octet-stream");
        }
    }
}