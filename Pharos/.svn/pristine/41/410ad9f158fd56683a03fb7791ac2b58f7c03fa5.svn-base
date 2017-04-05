using Pharos.POS.Retailing.Models;
using Pharos.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.POS.Retailing.Devices.CustomerScreens
{
    public class CustomerScreenFactory
    {
        public static ICustomerScreen Create()
        {
            if (Monitor.AllMonitors.Count() > 1)
            {
                return new LargeCustomerScreen();

            }
            else if (Global.MachineSettings.DevicesSettingsConfiguration.HasCustomerScreen)
            {
                return new ESC_POSCustomerScreen();
            }
            return null;
        }

    }
}
