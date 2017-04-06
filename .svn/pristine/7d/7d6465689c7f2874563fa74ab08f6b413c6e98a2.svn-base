using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.POS.ClientService
{
    public class StoreManageCenterConfig : ConfigurationSection
    {
        public static StoreManageCenterConfig GetConfig()
        {
            StoreManageCenterConfig section = (StoreManageCenterConfig)ConfigurationManager.GetSection("StoreManageCenterConfig");
            return section;
        }

        public static StoreManageCenterConfig GetConfig(string sectionName)
        {
            StoreManageCenterConfig section = (StoreManageCenterConfig)ConfigurationManager.GetSection("StoreManageCenterConfig");
            if (section == null)
                throw new ConfigurationErrorsException("Section " + sectionName + " is not found.");
            return section;
        }
        

        [ConfigurationProperty("Port", IsRequired = false, DefaultValue = 4002)]
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
    }
}