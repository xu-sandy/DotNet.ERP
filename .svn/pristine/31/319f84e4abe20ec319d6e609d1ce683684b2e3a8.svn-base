using Pharos.Logic.BLL.DataSynchronism;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Pharos.MSPP.Retailing.Controllers
{
    public class DataSynchronismController : ApiController
    {
        [HttpPost]
        public bool TestConnect()
        {
            return true;
        }

        [HttpPost]
        public bool UpLoad([FromBody]UpdateFormData info)
        {
            if (info != null)
            {
                return DataSyncContext.UpLoadAll(info);
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        public UpdateFormData Download([FromBody]UpdateFormData info)
        {
            if (info != null)
            {
                try
                {
                    return DataSyncContext.DownloadAll(info);
                }
                catch (Exception)
                {
                    return null;
                }

            }
            else
            {
                return null;
            }

        }
    }
}
