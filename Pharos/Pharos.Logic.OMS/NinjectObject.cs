﻿using Ninject;
using Pharos.Logic.OMS.BLL;
using Pharos.Logic.OMS.IDAL;
using Pharos.Logic.OMS.DAL;
using System.Web.Mvc;
using System;

namespace Pharos.Logic.OMS
{
    public class NinjectObject
    {
        /// <summary>
        /// 添加绑定
        /// </summary>
        /// <param name="kernel"></param>
        public static void AddBindings(IKernel kernel)
        {
            kernel.Bind(typeof(IBaseRepository<>)).To(typeof(BaseRepository<>));
            kernel.Bind<IProductRepository>().To<ProductRepository>();
            kernel.Bind<IDictRepository>().To<DictionaryRepository>();
            kernel.Bind<ITradersRepository>().To<TradersRepository>();
            kernel.Bind<IAgentsInfoRepository>().To<AgentsInfoRepository>();
            kernel.Bind<IAgentsRelationshipRepository>().To<AgentsRelationshipRepository>();
            kernel.Bind<IAgentsUsersRepository>().To<AgentsUsersRepository>();
            kernel.Bind<IBankCardInfoRepository>().To<BankCardInfoRepository>();
            kernel.Bind<IAgentPayRepository>().To<AgentPayRepository>();
            kernel.Bind<IPayLicenseRepository>().To<PayLicenseRepository>();
            kernel.Bind<IBankAccountRepository>().To<BankAccountRepository>();
            kernel.Bind<IApproveLogRepository>().To<ApproveLogRepository>();
            kernel.Bind<ITradersPayChannelRepository>().To<TradersPayChannelRepository>();
            kernel.Bind<ITradersStoreRepository>().To<TradersStoreRepository>();
            kernel.Bind<ITradersUserRepository>().To<TradersUserRepository>();
            kernel.Bind<ITradersPaySecretKeyRepository>().To<TradersPaySecretKeyRepository>();
            kernel.Bind<IProductModuleVerRepository>().To<ProductModuleVerRepository>();
            kernel.Bind<IProductRoleVerRepository>().To<ProductRoleVerRepository>();
            kernel.Bind<IProductDictionaryVerRepository>().To<ProductDictionaryVerRepository>();
            kernel.Bind<IProductDataVerRepository>().To<ProductDataVerRepository>();
            
            kernel.Bind<DevicesService>().ToSelf();
            kernel.Bind<AreaService>().ToSelf();
            kernel.Bind<CompAuthorService>().ToSelf();
            kernel.Bind<MenuService>().ToSelf();
            kernel.Bind<BrandService>().ToSelf();
            kernel.Bind<BusinessService>().ToSelf();
            kernel.Bind<ImportSetService>().ToSelf();
            kernel.Bind<ProductService>().ToSelf();
            kernel.Bind<ProductCategoryService>().ToSelf();
            kernel.Bind<LogService>().ToSelf();
            kernel.Bind<LogEngine>().ToSelf();
            kernel.Bind<SysMenuBLL>().ToSelf();
            kernel.Bind<PlanService>().ToSelf();

            var assby= System.Reflection.Assembly.GetExecutingAssembly();
            foreach(var type in assby.GetTypes())
            {
                if (!(type.Namespace!=null && type.Namespace.EndsWith("BLL",System.StringComparison.CurrentCultureIgnoreCase))) continue;
                if (kernel.TryGet(type) != null) continue;
                kernel.Bind(type).ToSelf();
            }

        }
        /// <summary>
        /// 从容器中获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T GetFromMVC<T>()
        {
            return DependencyResolver.Current.GetService<T>();
        }
    }
}
