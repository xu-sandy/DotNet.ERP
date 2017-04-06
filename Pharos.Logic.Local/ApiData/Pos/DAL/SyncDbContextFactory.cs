using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Pharos.Logic.DAL;

namespace Pharos.Logic.ApiData.Pos.DAL
{
    public static class SyncDbContextFactory
    {
        public static SyncDBContext Factory<SyncDBContext>()
            where SyncDBContext : DbContext, new()
        {
            return new SyncDBContext();
        }
    }
}
