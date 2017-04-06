using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;

namespace Pharos.OMS.Retailing
{
    /// <summary>
    /// IOC注入配置
    /// </summary>
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        public IKernel ninjectKernel{private set;get;}
        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            ninjectKernel.Settings.InjectNonPublic = true;
            Pharos.Logic.OMS.NinjectObject.AddBindings(ninjectKernel);
        }
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }
        
    }
}