using Pharos.POS.Retailing.Extensions;
using Pharos.POS.Retailing.Models;
using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.Controls;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Input;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharos.POS.Retailing
{
    /// <summary>
    /// MachineSettings.xaml 的交互逻辑
    /// </summary>
    public partial class MachineSettings : DialogWindow
    {
        public MachineSettings()
        {
            InitializeComponent();
            this.InitDefualtSettings();

            var collections = new ObservableCollection<ISettingsItem>();
            var machineInformations = Global.MachineSettings.MachineInformations;
            machineInformations.CurrentWindow = this;
            machineInformations.CurrentBindingElement = tabControl;

            var servicesConfiguration = Global.MachineSettings.ServicesConfiguration;
            servicesConfiguration.CurrentWindow = this;
            servicesConfiguration.CurrentBindingElement = tabControl;


            var devicesSettingsConfiguration = Global.MachineSettings.DevicesSettingsConfiguration;
            servicesConfiguration.CurrentWindow = this;
            servicesConfiguration.CurrentBindingElement = tabControl;


            collections.Add(servicesConfiguration);
            collections.Add(machineInformations);
            // collections.Add(devicesSettingsConfiguration);
            tabControl.SelectionChanged += tabControl_SelectionChanged;
            tabControl.ItemsSource = collections;
            this.Closing += MachineSettings_Closing;
            this.Loaded += MachineSettings_Loaded;
            if (string.IsNullOrEmpty(Global.MachineSettings.MachineInformations.DeviceId))
            {
                Task.Factory.StartNew(() =>
                {
                    var mac = Global.MachineSettings.MachineInformations.GetMACID(); ;
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        Global.MachineSettings.MachineInformations.DeviceId = mac;
                    }));
                });
            }
        }

        private void tabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (tabControl.SelectedItem != Global.MachineSettings.DevicesSettingsConfiguration && Global.MachineSettings.DevicesSettingsConfiguration.IsScanDeviceSetting)
            {
                Global.MachineSettings.DevicesSettingsConfiguration.IsScanDeviceSetting = false;

                Global.MachineSettings.DevicesSettingsConfiguration.ScanDeviceId = Global.MachineSettings.DevicesSettingsConfiguration.tempScanDeviceId;
                Global.MachineSettings.DevicesSettingsConfiguration.tempScanDeviceId = "";
            }
        }
        // private static RawKeyboard _keyboardDriver;

        void MachineSettings_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Owner == null)
                Application.Current.MainWindow = this;
        }

        void MachineSettings_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Global.MachineSettings.Enable && this.Owner == null)
            {

                Login win = new Login();
                win.Show();
            }

        }
    }
}
