using Pharos.Logic.ApiData.Pos;
using System;
#if (Local != true)
using System.Web;
using Pharos.Api.Retailing;
#endif

#if (Local != true)
[assembly: PreApplicationStartMethod(typeof(StoreConfig), "Register")]
#endif
namespace Pharos.Api.Retailing
{

    public class StoreConfig
    {
        /// <summary>
        /// 启动促销缓存刷新管理器
        /// </summary>
        public static void Register()
        {
#if (Local != true)
            StoreManager.InitStores(); 
#endif
#if(Local== true)
           var companyId = Convert.ToInt32( System.Configuration.ConfigurationManager.AppSettings["CompanyId"]);
            var storeId = System.Configuration.ConfigurationManager.AppSettings["StoreId"];
            StoreManager.SetUpStore(companyId, storeId);
#endif
        }
    }
}