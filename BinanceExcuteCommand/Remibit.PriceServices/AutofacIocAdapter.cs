using Autofac;
using Funq;
using ServiceStack.Configuration;
using System;
using Autofac.Integration.Mvc;

namespace Remibit.PriceServices
{
    public class AutofacIocAdapter : IContainerAdapter
    {
        private readonly IContainer _container;
        private readonly Container _funqContainer;

        public AutofacIocAdapter(IContainer container, Container funqContainer)
        {
            _container = container;
            // Register a RequestLifetimeScopeProvider (from Autofac.Integration.Mvc) with Funq
            var lifetimeScopeProvider = new RequestLifetimeScopeProvider(_container);
            funqContainer.Register<ILifetimeScopeProvider>(x => lifetimeScopeProvider);
            //Store the autofac application (root) container, and the funq container for later use            
            _funqContainer = funqContainer;
        }

        public Action<ContainerBuilder> ConfigAction { get; set; }

        private ILifetimeScope ActiveScope
        {
            get
            {
                // If there is an active HttpContext, retrieve the lifetime scope by resolving
                // the ILifetimeScopeProvider from Funq.  Otherwise, use the application (root) container.

                return _container;
            }
        }

        public T Resolve<T>()
        {
            return ActiveScope.Resolve<T>();
        }

        public T TryResolve<T>()
        {
            T result;

            if (ActiveScope.TryResolve<T>(out result))
            {
                return result;
            }

            return default(T);
        }
    }
}
