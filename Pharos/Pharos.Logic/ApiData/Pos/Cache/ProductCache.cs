﻿using Pharos.Logic.ApiData.Pos.DataAdapter;
using Pharos.Logic.ApiData.Pos.ValueObject;
using System;
using System.Linq;
using Pharos.Infrastructure.Data.Cache;
using Pharos.ObjectModels;
using Pharos.ObjectModels.DTOs;
using Pharos.Infrastructure.Data.Normalize;
using System.Collections.Generic;

namespace Pharos.Logic.ApiData.Pos.Cache
{
    public class ProductCache
#if(Local!=true)
: RedisCacheWrapper<ProductInfo>
#endif
#if(Local)
 : MemoryCacheWrapper<ProductInfo>
#endif
    {
        public ProductCache()
            : base("ProductCache", new TimeSpan(2, 0, 0), true)
        {
        }

        public static void RefreshProduct(IEnumerable<MemoryCacheRefreshQuery> _params)
        {
            var cache = DataAdapterFactory.ProductCache;
            foreach (var item in _params)
            {
                var storeIds = item.StoreId.Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (var storeId in storeIds)
                {
                    var key = KeyFactory.ProductKeyFactory(item.CompanyId, storeId, item.Barcode);
                    if (cache.ContainsKey(key))
                    {
                        RefreshProduct(key, item.CompanyId, item, storeId);
                    }
                }
            }
        }
        /// <summary>
        /// 重置缓存中的产品信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="companyId"></param>
        /// <param name="barcode"></param>
        /// <param name="storeId"></param>
        private static void RefreshProduct(string key, int companyId, MemoryCacheRefreshQuery _params, string storeId)
        {
            try
            {
                ProductInfo result = null;
                switch (_params.ProductType)
                {
                    case ProductType.Bundling:
                        {
                            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, storeId, string.Empty, companyId, string.Empty);
                            result = dataAdapter.GetProductInfoFromBundlingByBarcode(_params.Barcode);
                        }
                        break;
                    default:
                        {
                            var dataAdapter = DataAdapterFactory.DbFactory(MachinesSettings.Mode, storeId, string.Empty, companyId, string.Empty);
                            result = dataAdapter.GetProductInfoByBarcode(_params.Barcode);
                        }
                        break;
                }
                //加到缓存中
                if (result != null)
                    new ProductCache().Set(key, result);
            }
            catch { }
        }

    }
}