﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Pharos.SocketClient.Retailing.Protocol
{
    public class SocketClientConfig : ConfigurationSection
    {
        public static SocketClientConfig GetConfig()
        {
            SocketClientConfig section = (SocketClientConfig)ConfigurationManager.GetSection("SocketClientConfig");
            return section;
        }

        public static SocketClientConfig GetConfig(string sectionName)
        {
            SocketClientConfig section = (SocketClientConfig)ConfigurationManager.GetSection("SocketClientConfig");
            if (section == null)
                throw new ConfigurationErrorsException("Section " + sectionName + " is not found.");
            return section;
        }
        [ConfigurationProperty("IP", DefaultValue = "192.168.10.122", IsRequired = true)]
        public string Ip
        {
            get
            {
                return (string)base["IP"];
            }
            set
            {
                base["IP"] = value;
            }
        }

        [ConfigurationProperty("Port", IsRequired = false, DefaultValue = 2020)]
        public int Port
        {
            get
            {
                return (int)base["Port"];
            }
            set
            {
                base["Port"] = value;
            }
        }

        public string StoreId
        {
            get
            {
                return ConfigurationManager.AppSettings["StoreId"];
            }

        }
        public int CompanyId
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["CompanyId"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
    }
}
