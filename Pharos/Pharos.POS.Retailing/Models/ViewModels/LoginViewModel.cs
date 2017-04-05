﻿using Pharos.POS.Retailing.Devices.CustomerScreens;
using Pharos.POS.Retailing.Models.ApiParams;
using Pharos.POS.Retailing.Models.ApiReturnResults;
using Pharos.POS.Retailing.Models.PosModels;
using Pharos.Wpf.ViewModelHelpers;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            try
            {
                var images = Directory.GetFiles(@"Images\Login", "POS-login-*.png");
                var index = new Random().Next(images.Count() - 1);
                BackgroundImage = images[index];
            }
            catch
            {
            }
        }

        string logoIcon = @"Images\Login\logo_max.png";
        /// <summary>
        /// A WPF window always displays an icon. When one is not provided by setting Icon, WPF chooses an icon to display based on the following rules:
        ///Use the assembly icon, if specified.
        ///If the assembly icon is not specified, use the default Microsoft Windows icon.
        ///MSDN 内容，不允许动态修改来至外部的窗体图标
        /// </summary>
        public string LogoIcon
        {
            get
            {
                return logoIcon;
            }
            set
            {
                logoIcon = value;
                this.OnPropertyChanged(o => o.LogoIcon);
            }
        }

        string backgroundImage;
        public string BackgroundImage
        {
            get
            {
                return backgroundImage;
            }
            set
            {
                backgroundImage = value;
                this.OnPropertyChanged(o => o.BackgroundImage);
            }
        }

        string appName = string.Empty;
        public string AppName
        {
            get
            {
                return appName;
            }
            set
            {
                appName = value;
                this.OnPropertyChanged(o => o.AppName);
            }
        }

        string title = string.Empty;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                this.OnPropertyChanged(o => o.Title);
            }
        }

        string account = string.Empty;
        public string Account
        {
            get
            {
                return account;
            }
            set
            {
                account = value;
                this.OnPropertyChanged(o => o.Account);
            }
        }

        string password = string.Empty;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                this.OnPropertyChanged(o => o.Password);
            }
        }

        public GeneralCommand<object> LoginCommand
        {
            get
            {
                return new GeneralCommand<object>((_Params, command) =>
                {
                    var machineInfo = Global.MachineSettings.MachineInformations;
                    //if login
                    //登录接口，判断返回是否成功
                    LoginInfo _loginInfo = new LoginInfo() { StoreId = machineInfo.StoreId, MachineSn = machineInfo.MachineSn, Account = Account, Password = Password, DeviceSN = machineInfo.DeviceId, DeviceType = 1, EntryPoint = 0, CID = machineInfo.CompanyId, InTestMode = Convert.ToBoolean(_Params) };
                    var result = ApiManager.Post<object, ApiRetrunResult<LoginUserInfo>>(@"api/Login", _loginInfo);
                    if (result.Code == "200")
                    {
                        Global.CurrentSaleMen = result.Result;
                        var page = new PosWindow(AppName, LogoIcon, _loginInfo.InTestMode ? OperatingStatus.Test : OperatingStatus.Normal);
                        var customerScreen = CustomerScreenFactory.Create();
                        if (customerScreen != null)
                        {
                            customerScreen.Show(PosViewModel.Current);
                        }
                        page.Show();

                        CurrentWindow.Close();
                    }
                    else
                    {
                        Toast.ShowMessage(result.Message, CurrentWindow);
                    }

                });
            }
        }

        public GeneralCommand<object> SettingsCommand
        {
            get
            {
                return new GeneralCommand<object>((_Params, command) =>
                {
                    var win = new MachineSettings();
                    win.Owner = CurrentWindow;
                    win.ShowDialog();
                });
            }
        }

    }
}
