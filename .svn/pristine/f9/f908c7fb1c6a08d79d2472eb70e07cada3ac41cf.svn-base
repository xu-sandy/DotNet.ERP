using Pharos.AutoUpdateTools;
using Pharos.Wpf.ViewModelHelpers;
using System;
using System.Reflection;
using System.Xml.Serialization;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class ServicesConfiguration : BaseViewModel, ISettingsItem
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

        string xamlPath = "Templates/ServicesConfigurationTemplate.xaml";
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

        string header = "系统配置";
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
        [XmlIgnore]
        public string CurrentVersion
        {
            get
            {
                Assembly mainAssembly = Assembly.GetEntryAssembly();
                return mainAssembly.GetName().Version.ToString();
            }
        }

        bool serverPreferred;
        public bool ServerPreferred
        {
            get
            {
                return serverPreferred;
            }
            set
            {
                serverPreferred = value;
                this.OnPropertyChanged(o => o.ServerPreferred);
            }
        }
        string _ServerHostIp;
        public string ServerHostIp
        {
            get
            {
                return _ServerHostIp;
            }
            set
            {
                _ServerHostIp = value;
                this.OnPropertyChanged(o => o.ServerHostIp);
            }
        }
        int _ServerHostPort = 8012;
        public int ServerHostPort
        {
            get
            {
                return _ServerHostPort;
            }
            set
            {
                _ServerHostPort = value;
                this.OnPropertyChanged(o => o.ServerHostPort);
            }
        }
        string _LocalHostIp;
        public string LocalHostIp
        {
            get
            {
                return _LocalHostIp;
            }
            set
            {
                _LocalHostIp = value;
                this.OnPropertyChanged(o => o.LocalHostIp);
            }
        }
        int _LocalHostPort=8012;
        public int LocalHostPort
        {
            get
            {
                return _LocalHostPort;
            }
            set
            {
                _LocalHostPort = value;
                this.OnPropertyChanged(o => o.LocalHostPort);
            }
        }


        public string UpdatePath { get; set; }

        [XmlIgnore]
        public GeneralCommand<object> UpdateCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    if (string.IsNullOrEmpty(UpdatePath))
                    {
                        Toast.ShowMessage("无更新配置！", CurrentWindow);
                        return;
                    }
                    AutoUpdater.Start(UpdatePath);
                    AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
                    AutoUpdater.OpenDownloadPage = true;

                    AutoUpdater.LetUserSelectRemindLater = false;
                    AutoUpdater.RemindLaterTimeSpan = RemindLaterFormat.Days;
                    AutoUpdater.RemindLaterAt = 2;
                });
            }
        }

        private void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs _args)
        {
            CurrentWindow.Dispatcher.Invoke(new Action<UpdateInfoEventArgs>((args) =>
            {
                if (args != null)
                {
                    if (args.IsUpdateAvailable)
                    {
                        Confirm.ShowMessage(string.Format("发现新版本{0}，程序当前版本{1}，是否更新到最新版本？", args.CurrentVersion, args.InstalledVersion), CurrentWindow, (o) =>
                        {
                            if (o == ConfirmMode.Confirmed)
                            {
                                try
                                {
                                    AutoUpdater.DownloadUpdate();
                                }
                                catch (Exception exception)
                                {
                                    throw exception;
                                }
                            }
                        });
                    }
                    else
                    {
                        Toast.ShowMessage("未发现新版本，无需更新！", CurrentWindow);
                    }
                }
                else
                {
                    Toast.ShowMessage("程序更新发生问题，请检查是否联网！", CurrentWindow);
                }
            }), _args);
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

        internal void Reload(ServicesConfiguration servicesConfiguration)
        {
            ServerHostIp = servicesConfiguration.ServerHostIp;
            ServerHostPort = servicesConfiguration.ServerHostPort;
            LocalHostIp = servicesConfiguration.LocalHostIp;
            LocalHostPort = servicesConfiguration.LocalHostPort;
            ServerPreferred = servicesConfiguration.ServerPreferred;
            UpdatePath = servicesConfiguration.UpdatePath;
        }
        [XmlIgnore]
        public string ServerHost { get { return HostFormat(ServerHostIp, ServerHostPort); } }
        [XmlIgnore]
        public string LocalHost { get { return HostFormat(LocalHostIp, LocalHostPort); } }

        public string HostFormat(string ip, int port)
        {
            return string.Format("http://{0}:{1}/", ip, port);
        }
    }
}
