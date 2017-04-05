﻿using Pharos.Logic.DataSynchronism.ObjectModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Pharos.Logic.DataSynchronism
{
    internal class DataSyncDBContext : DbContext
    {
        static DataSyncDBContext()
        {
            //不重建数据库结构
            Database.SetInitializer<DataSyncDBContext>(null);
        }
        public DataSyncDBContext()
            : base(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"])
        {
            ///Leo: disable the Lazy Loading the WCF will not be able to serilize the entities.
            //是否启用延迟加载:  
            //  true:   延迟加载（Lazy Loading）：获取实体时不会加载其导航属性，一旦用到导航属性就会自动加载  
            //  false:  直接加载（Eager loading）：通过 Include 之类的方法显示加载导航属性，获取实体时会即时加载通过 Include 指定的导航属性  
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            //UseLegacyPreserveChangesBehavior
            //确定是否使用旧的行为， true 使用，false 不使用；
            this.Configuration.AutoDetectChangesEnabled = true;  //自动监测变化，默认值为 true 
        }

        public DbSet<DataSyncSaleOrders> SaleOrders { get; set; }
        
    }
}
