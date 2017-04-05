using Pharos.POS.Retailing.Devices.QuickConnectTools;
using Pharos.POS.Retailing.Models.PosModels;
using System.Threading;

namespace Pharos.POS.Retailing.Devices.POSDevices
{
    public interface IPOSDevice
    {
        POSDeviceType POSDeviceType { get; }
        bool DoPay(CancellationToken token, POSDevicePayRequest request, out POSDevicePayResponse response, out string msg);
    }
}
