using Pharos.Infrastructure.Data.Normalize;
using Pharos.Logic.ApiData.Pos.Cache;
using Pharos.Logic.ApiData.Pos.Sale;
using Pharos.Logic.ApiData.Pos.Sale.Suspend;
using Pharos.SocketClient.Retailing.CommandProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SocketClient.Retailing.Commands
{
    public class SyncShoppingCartCommand : BaseCommand
    {
        public SyncShoppingCartCommand()
            : base(new byte[4] { 0x00, 0x00, 0x00, 0x03 }, new PosStoreCommandNameProvider())
        {
        }
        public override void Execute(Protocol.Clients.PosStoreClient posStoreClient, Protocol.RequestInfos.PosStorePackageInfo package)
        {
            var jsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings();
            jsonSerializerSettings.Converters.Add(new BarcodeConverter());
            var shoppingcart = package.Read<ShoppingCart>(jsonSerializerSettings);
            if (shoppingcart != null)
            {
                var shoppingCartCache = new ShoppingCartCache();
                string key = KeyFactory.MachineKeyFactory(shoppingcart.MachineInformation.CompanyToken, shoppingcart.MachineInformation.StoreId, shoppingcart.MachineInformation.MachineSn, shoppingcart.MachineInformation.DeviceSn);
                shoppingCartCache.Set(key, shoppingcart);
            }
        }
    }
}
