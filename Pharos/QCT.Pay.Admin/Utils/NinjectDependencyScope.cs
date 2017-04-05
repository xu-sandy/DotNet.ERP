using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using Ninject.Syntax;
using Ninject;
namespace QCT.Pay.Admin
{
    public class NinjectDependencyScope : IDependencyScope
    {
        protected IResolutionRoot resolver{get;set;}
        internal NinjectDependencyScope(IResolutionRoot resolver)
        {
            this.resolver = resolver;
        }
        internal NinjectDependencyScope() { }
        public void Dispose()
        {
            resolver = null;
        }
        public object GetService(Type serviceType)
        {
            return resolver.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return resolver.GetAll(serviceType);
        }
    }
}
