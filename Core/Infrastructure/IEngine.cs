using Core.Config;
using Core.Infrastructure.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure {
    /// <summary>
    /// The interface of the engine. Handles
    /// the initialization of the project and
    /// the dependancies.
    /// </summary>
    public interface IEngine {
        /// <summary>
        /// The container manager that handles the depenecies.
        /// </summary>
        ContainerManager ContainerManager { get; }

        /// <summary>
        /// Initializes the engine.
        /// </summary>
        /// <param name="config">The engine configurations.</param>
        void Initialize(EngineConfig config);

        /// <summary>
        /// Resolves a dependency.
        /// </summary>
        /// <typeparam name="T">The type of the dependency.</typeparam>
        /// <returns>the instance of the resolved dependency.</returns>
        T Resolve<T>() where T : class;

        /// <summary>
        /// Resolves a dependency.
        /// </summary>
        /// <param name="type">The type of the dependency.</param>
        /// <returns>the object of the resolved dependency.</returns>
        object Resolve(Type type);

        /// <summary>
        /// Resolves a list of dependencies.
        /// </summary>
        /// <typeparam name="T">The type of the dependencies.</typeparam>
        /// <returns>a list with the instancies of the resolved dependencies.</returns>
        T[] ResolveAll<T>();
    }
}
