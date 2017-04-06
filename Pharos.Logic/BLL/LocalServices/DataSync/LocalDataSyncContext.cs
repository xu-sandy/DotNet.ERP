using Pharos.Logic.LocalEntity;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.LocalServices.DataSync
{
    public class LocalDataSyncContext
    {
        /// <summary>
        /// 服务器端数据同步服务对象
        /// </summary>
        public static Dictionary<string, dynamic> Entities { get; set; }

        public static Dictionary<string, string> TableNames { get; set; }

        public static Dictionary<string, IEnumerable<string>> UpdateBlocks { get; set; }

        static LocalDataSyncContext()
        {
            UpdateBlocks = new Dictionary<string, IEnumerable<string>>();
            TableNames = new Dictionary<string, string>();
            Entities = new Dictionary<string, dynamic>();
            //BASEINFO
            Mapping<SysDataDictionary>();
            Mapping<ProductInfo>();
            Mapping<ProductGroup>();
            Mapping<ProductBrand>();
            Mapping<ProductCategory>();
          //  Mapping<Commodity>();
            Mapping<SysStoreUserInfo>();
            Mapping<Members>();
            Mapping<ApiLibrary>();
            Mapping<Notice>();
            //DISCOUNT
            Mapping<CommodityPromotion>();
            Mapping<Bundling>();
            Mapping<BundlingList>();
            Mapping<CommodityDiscount>();
            Mapping<FreeGiftPurchase>();
            Mapping<FreeGiftPurchaseList>();
            Mapping<PromotionBlend>();
            Mapping<PromotionBlendList>();
            Mapping<DeviceRegInfo>();

            //SALE
            Mapping<SaleDetail>();
            Mapping<SaleDetailsTotal>();
            Mapping<SaleOrders>();
            Mapping<SalesReturns>();
            Mapping<MemberIntegral>();
            Mapping<PosIncomePayout>();
            Mapping<ConsumptionPayment>();
            Mapping<SalesReturnsDetailed>();
            Mapping<WipeZero>();

        }

        /// <summary>
        /// 映射数据同步领域上下文范围
        /// </summary>
        /// <typeparam name="LocalEntity">本地实体</typeparam>
        /// <typeparam name="TService">服务器端数据同步服务对象</typeparam>
        static void Mapping<LocalEntity>()
            where LocalEntity : Pharos.Logic.LocalEntity.BaseEntity, new()
        {
            var type = typeof(LocalEntity);
            var key = type.ToString();

            var titleAttr = Attribute.GetCustomAttribute(type, typeof(ExcelAttribute)) as ExcelAttribute;
            if (titleAttr != null)
                TableNames.Add(key, titleAttr.Title);
            Entities.Add(key, new LocalEntity());
        }

    }
}
