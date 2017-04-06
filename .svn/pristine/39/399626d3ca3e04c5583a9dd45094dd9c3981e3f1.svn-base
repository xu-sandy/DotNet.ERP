using Pharos.Logic.DAL.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.Logic.DAL
{
    public static class DbContextInitializer
    {
        /// <summary>
        /// 对于在应用程序中定义的每个DbContext类型，在首次使用时，Entity Framework都会根据数据库中的信息在内存生成一个映射视图（mapping views），而这个操作非常耗时。
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        public static void Initialize<TDbContext>() where TDbContext : DbContext, new()
        {
            Task.Factory.StartNew(() =>
            {
                var dbcontext = ContextFactory.GetCurrentContext<TDbContext>();
                var objectContext = ((IObjectContextAdapter)dbcontext).ObjectContext;
                var mappingCollection = (StorageMappingItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
                mappingCollection.GenerateViews(new List<EdmSchemaError>());
            });
        }
    }
}
