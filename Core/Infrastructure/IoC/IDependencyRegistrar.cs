using Autofac;
using Core.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.IoC {
    /// <summary>
    /// The interface for dependency registration
    /// across dlls.
    /// </summary>
    public interface IDependencyRegistrar {
        /// <summary>
        /// Register services and interfaces.
        /// </summary>
        /// <param name="builder">The AutoFac container builder.</param>
        /// <param name="typeFinder">A type finder helper.</param>
        /// <param name="config">The configuration settings.</param>
        void Register(ContainerBuilder builder, ITypeFinder typeFinder, EngineConfig config);

        /// <summary>
        /// Indicates in what order the dependecies will be added.
        /// </summary>
        int Order { get; }
    }
}
