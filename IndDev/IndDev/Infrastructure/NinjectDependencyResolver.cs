using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using System.Web.Mvc;
using IndDev.Domain.Abstract;
using IndDev.Domain.Context;
using Ninject;
using Ninject.Web.Common;

namespace IndDev.Infrastructure
{
    public class NinjectDependencyResolver : System.Web.Mvc.IDependencyResolver, System.Web.Http.Dependencies.IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver():this(new StandardKernel()) {}

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            AddBindings();
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose()
        {
            
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
            _kernel.Bind<ICustomer>().To<DbCustomer>().InRequestScope();
            _kernel.Bind<IMailRepository>().To<MailRepository>().InSingletonScope();
            _kernel.Bind<ISearchRepository>().To<DbSearch>().InSingletonScope();
            _kernel.Bind<ICrm>().To<CrmRepository>().InRequestScope();
        }
    }
}