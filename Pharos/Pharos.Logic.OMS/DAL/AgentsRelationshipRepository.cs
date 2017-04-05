﻿using Pharos.Logic.OMS.Entity;
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
    /// 代理商下级关系DAL
    /// </summary>
    public class AgentsRelationshipRepository : BaseRepository<AgentsRelationship>, IAgentsRelationshipRepository
    {
    }
}
