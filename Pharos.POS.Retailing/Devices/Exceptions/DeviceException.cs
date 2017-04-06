using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.POS.Retailing.Devices.Exceptions
{
    public class DeviceException : Exception
    {
        public DeviceException(string msg) : base(msg) { }
    }
}
