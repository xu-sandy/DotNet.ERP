using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Pharos.Sys.BLL;
using Pharos.Sys.DAL;

namespace Pharos.Sys
{
    public class NinjectData
    {
        static IKernel _kernel;
        /// <summary>
        /// 添加绑定
        /// </summary>
        /// <param name="kernel"></param>
        public static void AddBindings(IKernel kernel)
        {
            _kernel = kernel;
            kernel.Bind<OMSSysUserInfoBLL>().ToSelf();
            kernel.Bind(typeof(OMSCompanyAuthrizeDAL)).ToSelf();
        }

        internal static OMSCompanyAuthrizeDAL OMSCompanyAuthrizeDAL { get { return _kernel==null?new OMSCompanyAuthrizeDAL():(OMSCompanyAuthrizeDAL)_kernel.Get(typeof(OMSCompanyAuthrizeDAL)); } }
    }
}
