using QCT.Pay.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
using Pharos.Logic.OMS.BLL;

namespace Pharos.OMS.Retailing.Controllers
{
    public class PayApiController : ApiController
    {
        /// <summary>
        /// 支付交易Service
        /// </summary>
        [Ninject.Inject]
        PayForERPService PayERPSvc { get; set; }
        //
        // GET: /PayApi/

        #region 支付平台提供给ERP获取支付相关信息
        /// <summary>
        /// 根据商户ID获取支付方式列表
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public object GetPayModes(int cid)
        {
            var rst = PayERPSvc.GetMerchPayModes(cid);
            return rst.ToJson();
        }
        public object GetStorePayInfos(int cid)
        {
            var rst = PayERPSvc.GetMerchStorePayInfos(cid);
            return rst.ToJson();
        }
        #endregion
    }
}
