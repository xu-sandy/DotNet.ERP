﻿using Pharos.Logic.Mapping.LocalMapping;
using Pharos.Logic.LocalEntity;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Pharos.Logic.DAL.Sqlite
{
    public partial class SqliteDbContext : DbContext
    {
        static SqliteDbContext()
        {
            //不重建数据库结构
            Database.SetInitializer<SqliteDbContext>(null);
        }
        public SqliteDbContext()
            : base("SqliteConnectionString")
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ///移除EF映射默认给表名添加“s“或者“es”
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            ///配置关系

            modelBuilder.Configurations
                .Add(new SysStoreUserInfoMap())
                .Add(new NoticeMap())
                .Add(new ProductInfoMap())
              //  .Add(new CommodityMap())
                .Add(new SysDataDictionaryMap())
                .Add(new ApiLibraryMap())
                .Add(new BundlingMap())
                .Add(new BundlingListMap())
                .Add(new CommodityDiscountMap())
                .Add(new CommodityPromotionLocalMap())
                .Add(new ConsumptionPaymentMap())
                .Add(new FreeGiftPurchaseMap())
                .Add(new FreeGiftPurchaseListMap())
                .Add(new MemberIntegralMap())
                .Add(new PosIncomePayoutMap())
                .Add(new ProductBrandMap())
                .Add(new ProductCategoryMap())
                .Add(new PromotionBlendMap())
                .Add(new PromotionBlendListMap())
                .Add(new SaleDetailMap())
                .Add(new SaleDetailsTotalMap())
                .Add(new SaleOrdersMap())
                .Add(new SalesRecordMap())
                .Add(new SalesReturnsMap())
                .Add(new SalesReturnsDetailedMap())
                .Add(new PosCheckingMap())
                .Add(new ProductGroupMap())
                .Add(new WipeZeroMap())
                .Add(new DeviceRegInfoMap())
                .Add(new MembersMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
