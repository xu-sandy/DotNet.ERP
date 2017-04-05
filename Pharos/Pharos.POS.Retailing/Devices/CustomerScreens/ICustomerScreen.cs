using Pharos.POS.Retailing.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.POS.Retailing.Devices.CustomerScreens
{
    public interface ICustomerScreen
    {
        void Show(PosViewModel datas);
    }
}
