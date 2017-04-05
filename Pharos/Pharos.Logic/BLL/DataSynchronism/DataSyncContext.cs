﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.BLL.DataSynchronism.Services;
using Pharos.Logic.LocalEntity;
using Pharos.Logic.DAL;
using Newtonsoft.Json;

namespace Pharos.Logic.BLL.DataSynchronism
{
    /// <summary>
    /// 服务器端数据同步内容上下文
    /// </summary>
    public class DataSyncContext
    {
        /// <summary>
        /// 服务器端数据同步服务对象
        /// </summary>
        public static Dictionary<string, IDataSyncService> ServerSerivces { get; set; }

        static DataSyncContext()
        {

            ServerSerivces = new Dictionary<string, IDataSyncService>();
            //BASEINFO
            Mapping<SysDataDictionaryDataSyncService>("Pharos.Logic.LocalEntity.SysDataDictionary");
            Mapping<ProductRecordDataSyncService>("Pharos.Logic.LocalEntity.ProductInfo");
            Mapping<ProductGroupDataSyncService>("Pharos.Logic.LocalEntity.ProductGroup");
            Mapping<ProductRecordDataSyncService2>("Pharos.Logic.LocalEntity.ProductInfo2");
            Mapping<ProductBrandDataSyncService>("Pharos.Logic.LocalEntity.ProductBrand");
            Mapping<ProductCategoryDataSyncService>("Pharos.Logic.LocalEntity.ProductCategory");
           // Mapping<CommodityDataSyncService>("Pharos.Logic.LocalEntity.Commodity");
            Mapping<SysStoreUserInfoDataSyncService>("Pharos.Logic.LocalEntity.SysStoreUserInfo");
            Mapping<DeviceRegInfoDataSyncService>("Pharos.Logic.LocalEntity.DeviceRegInfo");
            Mapping<MembersDataSyncService>("Pharos.Logic.LocalEntity.Members");
            Mapping<ApiLibraryDataSyncService>("Pharos.Logic.LocalEntity.ApiLibrary");
            Mapping<NoticeDataSyncService>("Pharos.Logic.LocalEntity.Notice");
            //DISCOUNT
            Mapping<CommodityPromotionDataSyncService>("Pharos.Logic.LocalEntity.CommodityPromotion");
            Mapping<BundlingDataSyncService>("Pharos.Logic.LocalEntity.Bundling");
            Mapping<BundlingListDataSyncService>("Pharos.Logic.LocalEntity.BundlingList");
            Mapping<CommodityDiscountDataSyncService>("Pharos.Logic.LocalEntity.CommodityDiscount");
            Mapping<FreeGiftPurchaseDataSyncService>("Pharos.Logic.LocalEntity.FreeGiftPurchase");
            Mapping<FreeGiftPurchaseListDataSyncService>("Pharos.Logic.LocalEntity.FreeGiftPurchaseList");
            Mapping<PromotionBlendDataSyncService>("Pharos.Logic.LocalEntity.PromotionBlend");
            Mapping<PromotionBlendListDataSyncService>("Pharos.Logic.LocalEntity.PromotionBlendList");


            //SALE
            Mapping<SaleDetailDataSyncService>("Pharos.Logic.LocalEntity.SaleDetail");
            Mapping<SaleDetailsTotalDataSyncService>("Pharos.Logic.LocalEntity.SaleDetailsTotal");
            Mapping<SaleOrdersDataSyncService>("Pharos.Logic.LocalEntity.SaleOrders");
            Mapping<SalesReturnsDataSyncService>("Pharos.Logic.LocalEntity.SalesReturns");
            Mapping<MemberIntegralDataSyncService>("Pharos.Logic.LocalEntity.MemberIntegral");
            Mapping<PosIncomePayoutDataSyncService>("Pharos.Logic.LocalEntity.PosIncomePayout");
            Mapping<WipeZeroDataSyncService>("Pharos.Logic.LocalEntity.WipeZero");
            Mapping<ConsumptionPaymentDataSyncService>("Pharos.Logic.LocalEntity.ConsumptionPayment");
            Mapping<SalesReturnsDetailedDataSyncService>("Pharos.Logic.LocalEntity.SalesReturnsDetailed");
            Mapping<PosCheckingDataSyncService>("Pharos.Logic.LocalEntity.PosChecking");

        }

        /// <summary>
        /// 映射数据同步领域上下文范围
        /// </summary>
        /// <typeparam name="LocalEntity">本地实体</typeparam>
        /// <typeparam name="TService">服务器端数据同步服务对象</typeparam>
        static void Mapping<TService>(string localEntityType)
            where TService : IDataSyncService, new()
        {
            ServerSerivces.Add(localEntityType, new TService());
        }


        public static bool UpLoadAll(UpdateFormData datas)
        {
            var context = ContextFactory.GetCurrentContext<EFDbContext>();
            using (var localTransaction = context.Database.BeginTransaction())
            {
                
                try
                {
                    if (datas.Mode == DataSyncMode.UploadToServer)
                    {
                        foreach (var item in datas.Datas)
                        {
                            var service = DataSyncContext.ServerSerivces[item.Key];
                            var result = service.UpLoad(item.Key, JsonConvert.SerializeObject(item.Value), datas.StoreId,datas);
                            if (!result)
                            {
                                localTransaction.Rollback();
                                return false;
                            }
                        }
                    }
                    else if (datas.Mode == DataSyncMode.UpdateToServer)
                    {
                        foreach (var item in datas.Datas)
                        {
                            var service = DataSyncContext.ServerSerivces[item.Key];
                            if (!service.Update(item.Key, JsonConvert.SerializeObject(item.Value), datas.StoreId, datas))
                            {
                                localTransaction.Rollback();
                                return false;
                            }
                        }
                    } 


                    localTransaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    localTransaction.Rollback();
                    return false;
                }
            }
        }


        public static UpdateFormData DownloadAll(UpdateFormData datas)//POS端导出
        {
            UpdateFormData result = new UpdateFormData();
            foreach (var item in datas.Datas)
            {
                var service = DataSyncContext.ServerSerivces[item.Key];
                var returnDatas = service.Download(datas.StoreId, item.Key);
                result.Datas.Add(item.Key, returnDatas);
            }
            return result;
        }
        public static UpdateFormData ExportAll(UpdateFormData datas)//服务端导出
        {
            UpdateFormData result = new UpdateFormData();
            foreach (var item in datas.Datas)
            {
                var service = DataSyncContext.ServerSerivces[item.Key];
                var returnDatas = service.Export(datas.StoreId, item.Key);
                result.Datas.Add(item.Key, returnDatas);
            }
            return result;
        }
    }


}
