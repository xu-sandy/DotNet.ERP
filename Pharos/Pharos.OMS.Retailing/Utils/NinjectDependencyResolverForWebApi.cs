using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using Ninject;

namespace Pharos.OMS.Retailing
{
    public class NinjectDependencyResolverForWebApi : NinjectDependencyScope, IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolverForWebApi()
        {
            this.kernel = new StandardKernel();
            base.resolver = this.kernel;
            kernel.Settings.InjectNonPublic = true;
            Pharos.Logic.OMS.NinjectObject.AddBindings(kernel);
        }
        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(kernel);
        }
    }
}
