using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Pharos.Api.Retailing.Controllers
{
    [RoutePrefix("api")]
    public class UploadImageController : ApiController
    {
        /// <summary>
        /// 配置上传
        /// </summary>
        [HttpPost]
        [Route("UploadAppImage")]
        public void UploadAppImage()
        {
            var type = HttpContext.Current.Request["type"];
            var filename = HttpContext.Current.Request["filename"];
            var data= Request.Content.ReadAsByteArrayAsync().Result;
            string path="",path2="";
            if (type == "960")
                path = GetImgsController.GetImagePath("ios960");
            else
            {
                path = GetImgsController.GetImagePath("ios640");
                path2 = GetImgsController.GetImagePath("android640");
            }
            var fullname = Path.Combine(path, filename);
            using (var fs = new FileStream(fullname, FileMode.Create))
            {
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(data);
                bw.Close();
                fs.Close();
            }
            if(!string.IsNullOrEmpty(path2))
            {
                File.Copy(fullname, Path.Combine(path2, filename), true);
            }
        }
    }
}