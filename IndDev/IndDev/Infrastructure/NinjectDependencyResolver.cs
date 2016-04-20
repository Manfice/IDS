using System;
using System.Collections.Generic;
using System.Web.Mvc;
using IndDev.Domain.Abstract;
using IndDev.Domain.Context;
using Ninject;
using Ninject.Web.Common;

namespace IndDev.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            _kernel.Bind<INewsRepository>().To<DbNewsRepository>();
            _kernel.Bind<ISecureRepository>().To<DbSecureRepository>().InRequestScope();
            _kernel.Bind<IAdminRepository>().To<DbAdminRepository>().InRequestScope();
            _kernel.Bind<IShopRepository>().To<DbShopRepository>().InRequestScope();
            _kernel.Bind<IHomeRepository>().To<DbHomeRepository>().InRequestScope();
            _kernel.Bind<ICartRepository>().To<DbCart>().InSingletonScope();
            _kernel.Bind<ICustomer>().To<DbCustomer>().InSingletonScope();
            _kernel.Bind<IMailRepository>().To<MailRepository>().InSingletonScope();
        }
    }
}