using Pharos.AutoUpdateTools;
using Pharos.Wpf.ViewModelHelpers;
using System;
using System.Reflection;
using System.Xml.Serialization;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class DevicesSettingsConfiguration : BaseViewModel, ISettingsItem
    {
        string headerXamlPath = "Templates/DefaultTabControlHeaderTemplate.xaml";
        [XmlIgnore]
        public string HeaderXamlPath
        {
            get
            {
                return headerXamlPath;
            }
            set
            {
                headerXamlPath = value;
                this.OnPropertyChanged(o => o.HeaderXamlPath);
            }
        }

        string xamlPath = "Templates/DevicesSettingsConfigurationTemplate.xaml";
        [XmlIgnore]
        public string XamlPath
        {
            get
            {
                return xamlPath;
            }
            set
            {
                xamlPath = value;
                this.OnPropertyChanged(o => o.XamlPath);
            }
        }

        string header = "扫码枪设置";
        [XmlIgnore]
        public string Header
        {
            get
            {
                return header;
            }
            set
            {
                header = value;
                this.OnPropertyChanged(o => o.Header);
            }
        }
        bool _isScanDeviceSetting;
        public bool IsScanDeviceSetting
        {
            get
            {
                return _isScanDeviceSetting;
            }
            set
            {
                _isScanDeviceSetting = value;
                this.OnPropertyChanged(o => o.IsScanDeviceSetting);
            }
        }

        string _ScanDeviceId;
        public string ScanDeviceId
        {
            get
            {
                return _ScanDeviceId;
            }
            set
            {
                _ScanDeviceId = value;
                this.OnPropertyChanged(o => o.ScanDeviceId);
            }
        }

        string _BillPrintTpl = @"
<L@@10><F@@14><T@@门店：{STORE}><L@@10><T@@【{STOREID}】>
<L@@10><F@@14><T@@设备编号：{MACHINE}>
<L@@10><F@@14><T@@日期：{ORDERDATE@@yyyy-MM-dd HH:mm:ss}>
{table@@ORDERS}
<W@@200><F@@14><T@@{BARCODE}><L@@5><F@@14><T@@{PRODUCT}>
<W@@100><F@@14><T@@{PRICE}><W@@100><F@@14><T@@{TOTAL}>
{table@@ORDERS}";
        public string BillPrintTpl
        {
            get
            {
                return _BillPrintTpl;
            }
            set
            {
                _BillPrintTpl = value;
                this.OnPropertyChanged(o => o.BillPrintTpl);
            }
        }
        string _RefundOrChangingPrintTpl;
        public string RefundOrChangingPrintTpl
        {
            get
            {
                return _RefundOrChangingPrintTpl;
            }
            set
            {
                _RefundOrChangingPrintTpl = value;
                this.OnPropertyChanged(o => o.RefundOrChangingPrintTpl);
            }
        }
        string _DayReportPrintTpl;
        public string DayReportPrintTpl
        {
            get
            {
                return _DayReportPrintTpl;
            }
            set
            {
                _DayReportPrintTpl = value;
                this.OnPropertyChanged(o => o.DayReportPrintTpl);
            }
        }
        bool _HasCustomerScreen = false;
        public bool HasCustomerScreen
        {
            get
            {
                return _HasCustomerScreen;
            }
            set
            {
                _HasCustomerScreen = value;
                this.OnPropertyChanged(o => o.HasCustomerScreen);
            }
        }

        string _POSCOM = "COM1";
        public string POSCOM
        {
            get
            {
                return _POSCOM;
            }
            set
            {
                var comPort = value.ToUpper();
                if (!comPort.StartsWith("COM"))
                {
                    comPort = "COM" + comPort;
                }
                _POSCOM = comPort;

                this.OnPropertyChanged(o => o.POSCOM);
            }
        }
        int _POSTimeout = 60000;
        public int POSTimeout
        {
            get
            {
                return _POSTimeout;
            }
            set
            {
                _POSTimeout = value;
                this.OnPropertyChanged(o => o.POSTimeout);
            }
        }
        int _POSCOMRate = 57600;
        public int POSCOMRate
        {
            get
            {
                return _POSCOMRate;
            }
            set
            {
                _POSCOMRate = value;
                this.OnPropertyChanged(o => o.POSCOMRate);
            }
        }

        string _CustomerScreenCOMPort = "COM2";
        public string CustomerScreenCOMPort
        {
            get
            {
                return _CustomerScreenCOMPort;
            }
            set
            {
                _CustomerScreenCOMPort = value;
                this.OnPropertyChanged(o => o.CustomerScreenCOMPort);
            }
        }
        int _CustomerScreenCOMRate = 2400;
        public int CustomerScreenCOMRate
        {
            get
            {
                return _CustomerScreenCOMRate;
            }
            set
            {
                _CustomerScreenCOMRate = value;
                this.OnPropertyChanged(o => o.CustomerScreenCOMRate);
            }
        }



        internal string tempScanDeviceId = string.Empty;
        [XmlIgnore]
        public GeneralCommand<object> StartSetScanCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    try
                    {
                        IsScanDeviceSetting = !IsScanDeviceSetting;
                        if (IsScanDeviceSetting)
                        {
                            tempScanDeviceId = ScanDeviceId;
                            ScanDeviceId = "";
                        }
                        else
                        {
                            IsScanDeviceSetting = false;
                            tempScanDeviceId = string.Empty;
                            Global.MachineSettings.Save();
                            Toast.ShowMessage("保存成功！", CurrentWindow);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                });
            }
        }

        [XmlIgnore]
        public GeneralCommand<object> SaveCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    try
                    {
                        Global.MachineSettings.Save();
                        Toast.ShowMessage("保存成功！", CurrentWindow);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                });
            }
        }


        internal void Reload(DevicesSettingsConfiguration devicesSettingsConfiguration)
        {
            ScanDeviceId = devicesSettingsConfiguration.ScanDeviceId;
            CustomerScreenCOMRate = devicesSettingsConfiguration.CustomerScreenCOMRate;
            HasCustomerScreen = devicesSettingsConfiguration.HasCustomerScreen;
            CustomerScreenCOMPort = devicesSettingsConfiguration.CustomerScreenCOMPort;
            POSCOM = devicesSettingsConfiguration.POSCOM;
            POSCOMRate = devicesSettingsConfiguration.POSCOMRate;
            POSTimeout = devicesSettingsConfiguration.POSTimeout;
        }

    }
}
