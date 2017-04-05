﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Pharos.SocketClient.Retailing.Protocol
{
    class SocketClientConfig : ConfigurationSection
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
        [ConfigurationProperty("IP", IsRequired = true)]
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
        [ConfigurationProperty("StoreId", IsRequired = true)]
        public string StoreId
        {
            get
            {
                return (string)base["StoreId"];
            }
            set
            {
                base["StoreId"] = value;
            }
        }

        [ConfigurationProperty("CompanyId", IsRequired = false, DefaultValue = 1)]
        public int CompanyId
        {
            get
            {
                return (int)base["CompanyId"];
            }
            set
            {
                base["CompanyId"] = value;
            }
        }
    }
}
