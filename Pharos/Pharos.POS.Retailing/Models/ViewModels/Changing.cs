﻿using Pharos.Wpf.ViewModelHelpers;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class Changing : BaseViewModel, ISettingsItem
    {

        string headerXamlPath = "Templates/DefaultTabControlHeaderTemplate.xaml";
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

        string xamlPath = "Templates/HuanHuoTemplate.xaml";
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
    }
}
