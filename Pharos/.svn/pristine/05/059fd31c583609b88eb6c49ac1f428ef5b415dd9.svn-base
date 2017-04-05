using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using Pharos.Logic.ApiData.Pos.Sale.Barcodes;
using Newtonsoft.Json.Converters;
using System.Threading.Tasks;
using System.Threading;
using Pharos.Logic.ApiData.Pos.Exceptions;
using Pharos.Infrastructure.Data.Normalize;

namespace Pharos.Logic.ApiData.Pos.Sale.Suspend
{
    /// <summary>
    /// 挂单操作
    /// </summary>
    public class SaleSuspend
    {
        static SaleSuspend()
        {
            Expries = 1;
            Task.Factory.StartNew(() =>
            {
                while (true)
                {

                    Thread.Sleep(new TimeSpan(Expries, 0, 0, 0));
                }
            });
        }
        /// <summary>
        /// 挂单失效时间（默认为1天）
        /// </summary>
        public static int Expries { get; set; }
        public static void Suspend(ShoppingCart cart)
        {
            try
            {
                var statistics = cart.GetSaleStatistics();
                var details = new SuspendDetails()
                {
                    Id = KeyFactory.SuspendKeyFactory(cart.MachineInformation.CompanyId, cart.MachineInformation.StoreId, cart.MachineInformation.MachineSn, cart.MachineInformation.DeviceSn),
                    Amount = statistics.Receivable,
                    Count = statistics.Num,
                    SuspendDate = DateTime.Now,
                    OrderSN = cart.OrderSN
                };
                var suspendList = SuspendList.Factory(cart.MachineInformation.StoreId, cart.MachineInformation.MachineSn, cart.MachineInformation.CompanyId, MachinesSettings.CachePath);
                suspendList.Add(details);
                suspendList.Save(MachinesSettings.CachePath, cart.MachineInformation.StoreId, cart.MachineInformation.MachineSn, cart.MachineInformation.CompanyId);
                Save(cart, MachinesSettings.CachePath, details.Id);
                cart.Clear();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void Save(ShoppingCart cart, string path, string id)
        {
            var fileName = string.Format("SaleDetails{0}.JSON", id);
            var filePath = Path.Combine(path, fileName);

            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                JsonSerializer serializer = new JsonSerializer();
                JsonWriter writer = new JsonTextWriter(sw);
                serializer.Serialize(writer, cart.OrderList);
            }
        }


        public static IEnumerable<IBarcode> Read(ShoppingCart cart, string id)
        {
            try
            {
                List<IBarcode> result = null;
                var fileName = string.Format("SaleDetails{0}.JSON", id);
                var filePath = Path.Combine(MachinesSettings.CachePath, fileName);
                if (File.Exists(filePath))
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        var json = sr.ReadToEnd();
                        result = JsonConvert.DeserializeObject<List<IBarcode>>(json, new BarcodeConverter());
                    }
                }
                if (result != null && result.Count() > 0)
                {
                    cart.Clear();
                    cart.OrderList = result;
                    cart.ResetProduct(SaleStatus.Normal);
                }
                var suspendList = SuspendList.Factory(cart.MachineInformation.StoreId, cart.MachineInformation.MachineSn, cart.MachineInformation.CompanyId, MachinesSettings.CachePath);
                suspendList.RemoveAll(o => o.Id == id);
                suspendList.Save(MachinesSettings.CachePath, cart.MachineInformation.StoreId, cart.MachineInformation.MachineSn, cart.MachineInformation.CompanyId);
                File.Delete(filePath);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static SuspendList Remove(ShoppingCart cart, string id)
        {
            try
            {
                var suspendList = SuspendList.Factory(cart.MachineInformation.StoreId, cart.MachineInformation.MachineSn, cart.MachineInformation.CompanyId, MachinesSettings.CachePath);
                var fileName = string.Format("SaleDetails{0}.JSON", id);
                var filePath = Path.Combine(MachinesSettings.CachePath, fileName);
                suspendList.RemoveAll(o => o.Id == id);
                suspendList.Save(MachinesSettings.CachePath, cart.MachineInformation.StoreId, cart.MachineInformation.MachineSn, cart.MachineInformation.CompanyId);
                File.Delete(filePath);
                return suspendList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }


}
