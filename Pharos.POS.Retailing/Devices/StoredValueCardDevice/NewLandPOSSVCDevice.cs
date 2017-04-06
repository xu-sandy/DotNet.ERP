using Pharos.POS.Retailing.Devices.Exceptions;
using Pharos.POS.Retailing.Devices.POSDevices;
using Pharos.POS.Retailing.Devices.QuickConnectTools;
using Pharos.POS.Retailing.Models;
using Pharos.POS.Retailing.Models.PosModels;
using Pharos.POS.Retailing.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Pharos.POS.Retailing.Devices.StoredValueCardDevice
{
    public class NewLandPOSSVCDevice : IStoreValueCardDevice
    {
        public StoreValueCardDeviceType StoreValueCardDeviceType
        {
            get { return StoreValueCardDeviceType.NewLandPOSSVCDevice; }
        }

        public bool ReadCard(CancellationToken token, decimal amount, out Models.PosModels.StoreValueCardInfomactions info, out string msg)
        {
            info = null;
            msg = "";
            var request = new POSDevicePayRequest()
            {
                MachineSn = Global.MachineSettings.MachineInformations.MachineSn,
                Amount = amount,
                CashierId = PosViewModel.Current.UserCode,
                OldTransactionCode = "",
                OrderSn = PosViewModel.Current.OrderSn,
                Type = TransactionType.ReadCard
            };
            POSDevicePayResponse response = null;
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
                info = new StoreValueCardInfomactions() { CardNo = response.CardNo.Trim(), Password = response.CardPin.Trim() };
                return true;
            }
            catch (DeviceException ex)
            {
                msg = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                msg = "连接设备是失败！失败信息：" + ex.Message;
                return false;
            }

        }
    }
}
