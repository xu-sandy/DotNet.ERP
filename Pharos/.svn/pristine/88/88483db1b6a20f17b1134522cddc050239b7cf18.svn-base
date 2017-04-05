using System;
using System.Threading;
using Pharos.POS.Retailing.Models;
using Pharos.POS.Retailing.Models.PosModels;
using System.IO.Ports;
using Pharos.POS.Retailing.Devices.QuickConnectTools;
using Pharos.POS.Retailing.Devices.Exceptions;

namespace Pharos.POS.Retailing.Devices.POSDevices
{
    /// <summary>
    /// ME31
    /// </summary>
    public class NewLandPOSDevice : IPOSDevice
    {
        public bool DoPay(CancellationToken token, POSDevicePayRequest request, out POSDevicePayResponse response, out string msg)
        {
            msg = "";
            response = null;
            try
            {
                // return CallDevice(token, request, out response, out msg);
                SerialPort connection = NewLandPOSDeviceConnectTool.OpenSerialPort(token,
                    new SerialPortRequest()
                    {
                        ComPort = Global.MachineSettings.DevicesSettingsConfiguration.POSCOM,
                        BaudRate = Global.MachineSettings.DevicesSettingsConfiguration.POSCOMRate
                    });
                var cmd = NewLandPOSDeviceConnectTool.FormatDeviceCommand(request);
                NewLandPOSDeviceConnectTool.SendCommandToSerialPortConnection(token, connection, cmd);
                response = NewLandPOSDeviceConnectTool.ListenToSerialPortConnection(token, connection);
                msg = string.Format("成功支付{0}元！", request.Amount);
                return true;
            }
            catch (DeviceException ex)
            {
                msg = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                msg = "连接设备失败！" + ex.Message;
                return false;
            }
        }

        public POSDeviceType POSDeviceType
        {
            get { return POSDeviceType.NewLandPOSDevice; }
        }
    }
}
