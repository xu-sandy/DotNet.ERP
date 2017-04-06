using Pharos.DBFramework;
using Pharos.Sys.Entity;
using Pharos.Sys.Mapping;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace Pharos.Sys
{
    public partial class SysDbContext : DbContext
    {
        static SysDbContext()
        {
            //不重建数据库结构
            Database.SetInitializer<SysDbContext>(null);
        }
        public SysDbContext()
            : base(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"])
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
            /////配置关系
            modelBuilder.Configurations
                .Add(new SysDepartmentsMap());

            base.OnModelCreating(modelBuilder);
        }

        ///// <summary>
        ///// 数据字典
        ///// </summary>
        //public DbSet<SysDataDictionary> SysDataDictionaries { get; set; }
        ///// <summary>
        ///// 角色
        ///// </summary>
        //public DbSet<SysRoles> SysRoles { get; set; }
        ///// <summary>
        ///// 系统日志
        ///// </summary>
        //public DbSet<SysLog> SysLog { get; set; }
        ///// <summary>
        ///// 用户信息
        ///// </summary>
        //public DbSet<SysUserInfo> SysUserInfos { get; set; }
        ///// <summary>
        ///// 系统菜单
        ///// </summary>
        //public DbSet<SysMenus> SysMenus { get; set; }
        ///// <summary>
        ///// 系统权限
        ///// </summary>
        //public DbSet<SysLimits> SysLimits { get; set; }
        ///// <summary>
        ///// 机构部门
        ///// </summary>
        //public DbSet<SysDepartments> SysDepartments { get; set; }
        ///// <summary>
        ///// 自定义菜单
        ///// </summary>
        //public DbSet<SysCustomMenus> SysCustomMenus { get; set; }
        ///// <summary>
        ///// 自定义用户权限
        ///// </summary>
        //public DbSet<SysUsersLimits> SysUsersLimits { get; set; }

    }
}