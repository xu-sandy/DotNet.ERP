using Pharos.POS.Retailing.Models;
using Pharos.POS.Retailing.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows;

namespace Pharos.POS.Retailing.Devices.CustomerScreens
{
    public class ESC_POSCustomerScreen : ICustomerScreen
    {
        private PosViewModel Datas { get; set; }
        public void Show(Models.ViewModels.PosViewModel datas)
        {
            Datas = datas;
            datas.PropertyChanged += datas_PropertyChanged;
        }

        void datas_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Receivable")
            {
                try
                {

                    SerialPort serialPort1 = new SerialPort();
                    serialPort1.PortName = Global.MachineSettings.DevicesSettingsConfiguration.CustomerScreenCOMPort;
                    serialPort1.BaudRate = Global.MachineSettings.DevicesSettingsConfiguration.CustomerScreenCOMRate;
                    serialPort1.Open();
                    List<byte> data1 = new List<byte>();
                    data1.Add(027);
                    data1.Add(081);
                    data1.Add(065);
                    data1.AddRange(Encoding.UTF8.GetBytes(Datas.Receivable.ToString("N2")));
                    data1.Add(013);
                    string str = Encoding.UTF8.GetString(data1.ToArray());
                    serialPort1.WriteLine(str);
                    serialPort1.Close();
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
