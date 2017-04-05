using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.LocalServices
{
    public interface IDiscountService<TDAO>
    {
        IEnumerable<TDAO> LoadDiscount();
    }
}
