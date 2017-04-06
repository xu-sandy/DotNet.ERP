using Pharos.POS.Retailing.Models.PosModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Pharos.POS.Retailing.Devices.StoredValueCardDevice
{
    public interface IStoreValueCardDevice
    {
        StoreValueCardDeviceType StoreValueCardDeviceType { get; }
        bool ReadCard(CancellationToken token,decimal amount, out Models.PosModels.StoreValueCardInfomactions info, out string msg);
    }
}
