using Newtonsoft.Json;
using Pharos.Infrastructure.Data.Normalize;
using Pharos.Infrastructure.Data.Redis;
using Pharos.Logic.ApiData.Pos.Cache;
using Pharos.Logic.ApiData.Pos.Exceptions;
using Pharos.Logic.ApiData.Pos.Sale.Suspend;
using Pharos.Logic.ApiData.Pos.User;
using Pharos.Logic.ApiData.Pos.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Sale
{
    /// <summary>
    /// 购物车工厂
    /// </summary>
    public static class ShoppingCartFactory
    {
        public static ShoppingCartCache shoppingCartCache = new ShoppingCartCache();

        public static ShoppingCart Factory(string storeId, string machineId, int companyId, string deviceSn, bool isNew = false)
        {
            string key = KeyFactory.MachineKeyFactory(companyId, storeId, machineId, deviceSn);

            if (shoppingCartCache.ContainsKey(key) && !isNew)
            {
#if(Local!=true)
                shoppingCartCache.JsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings();
                shoppingCartCache.JsonSerializerSettings.Converters.Add(new BarcodeConverter());
#endif
                var shoppingCart = shoppingCartCache.Get(key);
                return shoppingCart;
            }
            else
            {
                shoppingCartCache.Remove(key);
#if(Local!=true)

                OnlineCache onlineCache = new OnlineCache();
#endif
#if(Local)
                OnlineCache onlineCache = Salesclerk.onlineCache;
#endif

                var machineInfo = onlineCache.Get(key);
                if (machineInfo != null)
                {
                    var shoppingCart = new ShoppingCart();
                    shoppingCart.MachineInformation = machineInfo;
                    shoppingCart.NewAndResetOrderSN();
                    shoppingCartCache.Set(key, shoppingCart);
                    return shoppingCart;
                }
                else
                {
                    goto TheSaleException;
                }
            }

        TheSaleException:
            throw new SaleException("400", "未登录或者未授权，不允许操作！");
        }

        public static void ResetCache(ShoppingCart shoppingCart, string storeId, string machineId, int companyId, string deviceSn)
        {
            string key = Pharos.Infrastructure.Data.Normalize.KeyFactory.MachineKeyFactory(companyId, storeId, machineId, deviceSn);
            shoppingCartCache.Set(key, shoppingCart);
           // RedisManager.Publish("SyncShoppingCartCache", JsonConvert.SerializeObject(shoppingCart));
        }
    }
}
