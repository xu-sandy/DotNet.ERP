﻿using Pharos.POS.Retailing.Models.ApiParams;
using Pharos.POS.Retailing.Models.ApiReturnResults;
using Pharos.Wpf.ViewModelHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;
using System.Xml.Serialization;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class MachineInformations : BaseViewModel, ISettingsItem
    {
        internal string GetMACID()
        {
            return GetMacByIPConfig().FirstOrDefault();
        }
        private List<string> GetMacByIPConfig()
        {
            List<string> macs = new List<string>();
            ProcessStartInfo startInfo = new ProcessStartInfo("ipconfig", "/all");
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            Process p = Process.Start(startInfo);
            //截取输出流
            StreamReader reader = p.StandardOutput;
            string line = reader.ReadLine();

            while (!reader.EndOfStream)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    line = line.Trim();

                    if (line.StartsWith("Physical Address") || line.StartsWith("物理地址"))
                    {
                        macs.Add(line.Split(':')[1].Replace("-", "").Trim());
                    }
                }

                line = reader.ReadLine();
            }

            //等待程序执行完退出进程
            p.WaitForExit();
            p.Close();
            reader.Close();

            return macs;
        }


        internal void Reload(MachineInformations machineInformations)
        {
            MachineSn = machineInformations.MachineSn;
            StoreId = machineInformations.StoreId;
            StoreName = machineInformations.StoreName;
            Phone = machineInformations.Phone;
            Company = machineInformations.Company;
            CompanyId = machineInformations.CompanyId;
            PrintStatus = machineInformations.PrintStatus;
            DeviceId = machineInformations.DeviceId;
            IsNonCashWipeZero = machineInformations.IsNonCashWipeZero;
        }


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

        string xamlPath = "Templates/MachineInformationsTemplate.xaml";
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

        string header = "POS配置";
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

        string machineSn;
        public string MachineSn
        {
            get
            {
                return machineSn;
            }
            set
            {
                if (!Regex.IsMatch(value, "^[0-9][0-9]$") && CurrentWindow != null)
                {
                    Toast.ShowMessage("POS机号限01-99范围！", CurrentWindow);
                    return;
                }
                if (value.Length == 1)
                {
                    machineSn = "0" + value;
                }
                else
                {
                    machineSn = value;
                }
                this.OnPropertyChanged(o => o.MachineSn);
            }
        }
        int companyToken;
        public int CompanyId
        {
            get
            {
                return companyToken;
            }
            set
            {
                companyToken = value;
                this.OnPropertyChanged(o => o.CompanyId);
            }
        }

        string deviceId;
        public string DeviceId
        {
            get
            {
                return deviceId;
            }
            set
            {
                deviceId = value.ToUpper();
                this.OnPropertyChanged(o => o.DeviceId);
            }
        }

        string storeId;
        public string StoreId
        {
            get
            {
                return storeId;
            }
            set
            {
                storeId = value;
                if (CurrentWindow != null)
                {
                    Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            BaseApiParams _deviceInfo = new BaseApiParams() { MachineSn = machineSn, StoreId = storeId, CID = CompanyId };
                            var result = ApiManager.Post<object, ApiRetrunResult<string>>(@"/api/GetStoreName", _deviceInfo);
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                if (result.Code == "200")
                                {
                                    StoreName = result.Result;
                                }
                                else
                                {
                                    StoreName = string.Empty;
                                    Toast.ShowMessage(new ToastMessage() { Message = result.Message, Seconds = 5 }, CurrentWindow);
                                }

                            }));

                        }
                        catch
                        {


                        }
                    });
                }
                this.OnPropertyChanged(o => o.StoreId);
            }
        }

        string company;
        public string Company
        {
            get
            {
                return company;
            }
            set
            {
                company = value;
                this.OnPropertyChanged(o => o.Company);
            }
        }
        int qianXiangType = 0;

        public int QianXiangType
        {
            get
            {
                return qianXiangType;
            }
            set
            {
                qianXiangType = value;
                this.OnPropertyChanged(o => o.QianXiangType);
            }
        }

        string storeName;
        public string StoreName
        {
            get
            {
                return storeName;
            }
            set
            {
                storeName = value;
                this.OnPropertyChanged(o => o.StoreName);
            }
        }
        string phone;
        public string Phone
        {
            get
            {
                return phone;
            }
            set
            {
                phone = value;
                this.OnPropertyChanged(o => o.Phone);
            }
        }

        /// <summary>
        /// 小票状态
        /// </summary>
        private PrintStatus printStatus = PrintStatus.Close;

        public PrintStatus PrintStatus
        {
            get { return printStatus; }
            set
            {
                printStatus = value;
                this.OnPropertyChanged(o => o.PrintStatus);
            }
        }


        /// <summary>
        /// 设备注册
        /// </summary>
        [XmlIgnore]
        public GeneralCommand<object> DeviceRegisterCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    DeviceRegInfo _deviceInfo = new DeviceRegInfo() { MachineSn = machineSn, DeviceSN = deviceId, StoreId = storeId, CID = CompanyId };

                    var result = ApiManager.Post<object, ApiRetrunResult<object>>(@"api/RegisterDevice", _deviceInfo);
                    if (result.Code == "200")
                    {
                        Toast.ShowMessage("设置成功！", CurrentWindow);
                    }
                    else
                    {
                        Toast.ShowMessage(result.Message, CurrentWindow);
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
                        if (!Regex.IsMatch(StoreId, "^[1-9][0-9]{0,2}$"))
                        {
                            Toast.ShowMessage("SID限1-999范围！", CurrentWindow);
                            return;
                        }

                        if (!Regex.IsMatch(MachineSn, "^[0-9][0-9]$"))
                        {
                            Toast.ShowMessage("POS机号限01-99范围！", CurrentWindow);
                            return;
                        }

                        if (string.IsNullOrEmpty(StoreName))
                        {
                            Toast.ShowMessage("请通过设置正确的SID，以获取门店名称！", CurrentWindow);
                            return;
                        }
                        Global.MachineSettings.Save();


                        //     Toast.ShowMessage("保存成功，正在注册设备！", CurrentWindow);
                        DeviceRegisterCommand.Execute(null);
                    }
                    catch (Exception ex)
                    {
                        Toast.ShowMessage(ex.Message, CurrentWindow);
                        throw ex;
                    }
                });
            }
        }
        [XmlIgnore]
        public GeneralCommand<object> CopyCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    if (!string.IsNullOrEmpty(DeviceId))
                        Clipboard.SetText(DeviceId);
                });
            }
        }

        /// <summary>
        /// 非现金抹零
        /// </summary>
        private bool isNonCashWipeZero = true;

        public bool IsNonCashWipeZero
        {
            get { return isNonCashWipeZero; }
            set
            {
                isNonCashWipeZero = value;
                this.OnPropertyChanged(o => o.IsNonCashWipeZero);
            }
        }


    }
}
