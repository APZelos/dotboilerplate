using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using Autofac.Core;
using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc;
using Core.Utilities;

namespace Core.Infrastructure.IoC {
    /// <summary>
    /// Wraps the AutoFac container.
    /// </summary>
    public class ContainerManager {
        #region Fields and Properties

        /// <summary>
        /// The instance of the AutoFac container.
        /// </summary>
        public IContainer Container { get; }

        #endregion Fields and Properties

        /// <summary>
        /// The default constructor.
        /// Sets the instance of the AutoFac container.
        /// </summary>
        /// <param name="container">The AutoFac container.</param>
        public ContainerManager(IContainer container) {
            Container = container;
        }

        #region Public Methods
        /// <summary>
        /// The current scope.
        /// </summary>
        /// <returns>the current Scope</returns>
        public virtual ILifetimeScope Scope() {
            try {
                if (HttpContext.Current.IsNotNull())
                    return AutofacDependencyResolver.Current.RequestLifetimeScope;

                //when such lifetime scope is returned, you should be sure that it'll be disposed once used (e.g. in schedule tasks)
                return Container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            } catch (Exception) {
                //we can get an exception here if RequestLifetimeScope is already disposed
                //for example, requested in or after "Application_EndRequest" handler
                //but note that usually it should never happen

                //when such lifetime scope is returned, you should be sure that it'll be disposed once used (e.g. in schedule tasks)
                return Container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
        }

        /// <summary>
        /// Retrieve a service from the context.
        /// </summary>
        /// <typeparam name="T">The type of the service we want to retrieve.</typeparam>
        /// <param name="key">The key the service is stored with. Default: "".</param>
        /// <param name="scope">The Scope that the service is stored in. Null for current scope. Default: null.</param>
        /// <returns>the resolved service.</returns>
        public virtual T Resolve<T>(string key = "", ILifetimeScope scope = null) where T : class {
            scope = scope ?? Scope();

            if (key.IsNullOrEmpty())
                return scope.Resolve<T>();

            return scope.ResolveKeyed<T>(key);
        }

        /// <summary>
        /// Retrieve a service from the context.
        /// </summary>
        /// <param name="type">The type of the service we want to retrieve.</param>
        /// <param name="scope">The Scope that the service is stored in. Null for current scope. Default: null.</param>
        /// <returns>the resolved service.</returns>
        public virtual object Resolve(Type type, ILifetimeScope scope = null) {
            scope = scope ?? Scope();
            return scope.Resolve(type);
        }

        /// <summary>
        /// Retrieve a list of services from the context.
        /// </summary>
        /// <typeparam name="T">The type of the services we want to retrieve.</typeparam>
        /// <param name="key">The key the services are stored with. Default: "".</param>
        /// <param name="scope">The Scope that the services are stored in. Null for current scope. Default: null.</param>
        /// <returns>an enumerable of the resolve services.</returns>
        public virtual T[] ResolveAll<T>(string key = "", ILifetimeScope scope = null) {
            scope = scope ?? Scope();

            if (key.IsNullOrEmpty())
                return scope.Resolve<IEnumerable<T>>().ToArray();

            return scope.ResolveKeyed<IEnumerable<T>>(key).ToArray();
        }

        /// <summary>
        /// Resolve a not registered type.
        /// </summary>
        /// <typeparam name="T">The type of the service we want to retrieve.</typeparam>
        /// <param name="scope">The Scope that the service is stored in. Null for current scope. Default: null.</param>
        /// <returns>the resolved service.</returns>
        public virtual T ResolveUnregistered<T>(ILifetimeScope scope = null) where T : class 
            => ResolveUnregistered(typeof(T), scope) as T;

        /// <summary>
        /// Resolve a not registered type.
        /// </summary>
        /// <param name="type">The type of the service we want to retrieve.</param>
        /// <param name="scope">The Scope that the service is stored in. Null for current scope. Default: null.</param>
        /// <returns>the resolved service.</returns>
        public virtual object ResolveUnregistered(Type type, ILifetimeScope scope = null) {
            scope = scope ?? Scope();
            // Loops all the constructions of the type until it finds one that
            // can resolve all of its parameters and create an instance of the
            // type with this constructor.
            var constructors = type.GetConstructors();
            foreach (var constructor in constructors) {
                try {
                    var parameters = constructor.GetParameters();
                    var parameterInstances = new List<object>();
                    foreach (var parameter in parameters) {
                        var service = Resolve(parameter.ParameterType, scope);
                        if (service.IsNull())
                            throw new Exception("Unknown dependency");
                        parameterInstances.Add(service);
                    }
                    return Activator.CreateInstance(type, parameterInstances.ToArray());
                } catch (Exception) {

                }
            }
            throw new Exception("No constructor  was found that had all the dependencies satisfied.");
        }

        /// <summary>
        /// Try to resolve service.
        /// </summary>
        /// <param name="type">The type of the service we want to retrieve.</param>
        /// <param name="scope">The Scope that the service is stored in. Null for current scope. Default: null.</param>
        /// <param name="instance">An object that will hold the resolved service.</param>
        /// <returns>true if the service was resolved successfully.</returns>
        public virtual bool TryResolve(Type type, ILifetimeScope scope, out object instance) {
            scope = scope ?? Scope();
            return scope.TryResolve(type, out instance);
        }

        /// <summary>
        /// Determinate whether the specified service exist in the context.
        /// </summary>
        /// <param name="type">The type of the service we want to check.</param>
        /// <param name="scope">The Scope that the service is stored in. Null for current scope. Default: null.</param>
        /// <returns>true if the service is registered in the context.</returns>
        public virtual bool IsRegistered(Type type, ILifetimeScope scope = null) {
            scope = scope ?? Scope();
            return scope.IsRegistered(type);
        }

        /// <summary>
        /// Retrieve a service from the context if is registered.
        /// </summary>
        /// <param name="type">The type of the service we want to retrieve.</param>
        /// <param name="scope">The Scope that the service is stored in. Null for current scope. Default: null.</param>
        /// <returns>the resolved service if is registered or null if not.</returns>
        public virtual object ResolveOptional(Type type, ILifetimeScope scope = null) {
            scope = scope ?? Scope();
            return scope.ResolveOptional(type);
        }
        #endregion Public Methods
    }
}
