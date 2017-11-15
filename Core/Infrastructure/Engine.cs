using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Config;
using Core.Infrastructure.IoC;
using Autofac;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using Core.Infrastructure.Mapper;
using AutoMapper;

namespace Core.Infrastructure {
    public class Engine : IEngine {
        #region Fields and Properties
        /// <summary>
        /// The container manager that handles the depenecies.
        /// </summary>
        public ContainerManager ContainerManager { get; private set; }
        #endregion Fields and Properties

        #region Public Methods
        /// <summary>
        /// Initializes the engine.
        /// </summary>
        /// <param name="config">The engine configurations.</param>
        public void Initialize(EngineConfig config) {
            RegisterDependencies(config);
            RegisterMapperConfiguration(config);
        }

        /// <summary>
        /// Resolves a dependency.
        /// </summary>
        /// <typeparam name="T">The type of the dependency.</typeparam>
        /// <returns>the instance of the resolved dependency.</returns>
        public T Resolve<T>() where T : class {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Resolves a dependency.
        /// </summary>
        /// <param name="type">The type of the dependency.</param>
        /// <returns>the object of the resolved dependency.</returns>
        public object Resolve(Type type) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Resolves a list of dependencies.
        /// </summary>
        /// <typeparam name="T">The type of the dependencies.</typeparam>
        /// <returns>a list with the instancies of the resolved dependencies.</returns>
        public T[] ResolveAll<T>() {
            throw new NotImplementedException();
        }
        #endregion Public Methods

        #region Private Methods
        /// <summary>
        /// Registers all teh dependencies.
        /// </summary>
        /// <param name="config">The engine configurations.</param>
        protected virtual void RegisterDependencies(EngineConfig config) {
            var builder = new ContainerBuilder();
            var typeFinder = new WebAppTypeFinder();

            // Core single instance dependencies.
            builder.RegisterInstance(config).As<EngineConfig>().SingleInstance();
            builder.RegisterInstance(this).As<IEngine>().SingleInstance();
            builder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();

            // Collects and registers dependencies from all the assemblies.
            var dependencyRegistarTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            var dependencyRegistarInstances = dependencyRegistarTypes
                .Select(type => (IDependencyRegistrar)Activator.CreateInstance(type));
            dependencyRegistarInstances.OrderBy(instance => instance.Order);
            foreach (var instance in dependencyRegistarInstances)
                instance.Register(builder, typeFinder, config);

            // Initialize ContainerManager
            var container = builder.Build();
            ContainerManager = new ContainerManager(container);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        protected virtual void RegisterMapperConfiguration(EngineConfig config) {
            var typeFinder = new WebAppTypeFinder();

            // Collects and registers dependencies from all the assemblies.
            var mapConfigTypes = typeFinder.FindClassesOfType<IMapConfig>();
            var mapConfigInstances = mapConfigTypes
                .Select(type => (IMapConfig)Activator.CreateInstance(type));
            var configurationActions = mapConfigInstances
                .OrderBy(instance => instance.Order)
                .Select(instance => instance.GetConfiguration())
                .ToList();
        }
        #endregion Private Methods
    }
}
