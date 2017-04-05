using Pharos.Logic.OMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharos.Logic.OMS.Entity.View;
using System.Data;
using System.Data.SqlClient;
using Pharos.Logic.OMS.IDAL;

namespace Pharos.Logic.OMS.DAL
{
    /// <summary>
    /// DAL商家支付通道
    /// </summary>
    public class TradersPayChannelRepository : BaseRepository<TradersPayChannel>, ITradersPayChannelRepository
    {
    }
}
