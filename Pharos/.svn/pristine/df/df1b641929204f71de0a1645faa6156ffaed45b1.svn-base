using Pharos.Frame.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Pharos.Barcode.Retailing.Models
{
    public class SystemConfiguration : BaseViewModel
    {
        static SystemConfiguration()
        {
            Current = new SystemConfiguration();
            var url = ConfigurationManager.AppSettings["ServerUrl"];
            if (string.IsNullOrEmpty(url))
            {
                //todo: warning message;
                return;
            }
            Current.ServerUrl = url;
        }
        public SystemConfiguration()
        {

        }
        public string ServerUrl { get; set; }

        public static SystemConfiguration Current { get; set; }

        public GeneralCommand<object> SaveCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    UpdateAppConfig("ServerUrl", ServerUrl);
                });
            }
        }
        private static void UpdateAppConfig(string newKey, string newValue)
        {
            bool isModified = false;
            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key == newKey)
                {
                    isModified = true;
                }
            }

            Configuration config =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (isModified)
            {
                config.AppSettings.Settings.Remove(newKey);
            }
            config.AppSettings.Settings.Add(newKey, newValue);
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
