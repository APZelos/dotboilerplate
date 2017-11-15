using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Core.Infrastructure.Mapper {
    /// <summary>
    /// The base class for configuring mapping
    /// across dlls.
    /// </summary>
    /// <typeparam name="S">The type of the source object.</typeparam>
    /// <typeparam name="D">The type of the destination object.</typeparam>
    public abstract class BaseMapConfig<S, D> : IMapConfig {

        /// <summary>
        /// Get the configurations.
        /// </summary>
        /// <returns>a mapping configuration mapping.</returns>
        public Action<IMapperConfigurationExpression> GetConfiguration() {
            Action<IMapperConfigurationExpression> action = cfg => {
                MapSourceToDestinationConfig(cfg.CreateMap<S, D>());
                MapDestinationToSourceConfig(cfg.CreateMap<D, S>());
            };

            return action;
        }

        /// <summary>
        /// Configures the mapping of the source object to the destination object.
        /// </summary>
        /// <param name="mapping">The mapping object.</param>
        public abstract void MapSourceToDestinationConfig(IMappingExpression<S, D> mapping);

        /// <summary>
        /// Configures the mapping of the destination object to the source object.
        /// </summary>
        /// <param name="mapping">The mapping object.</param>
        public abstract void MapDestinationToSourceConfig(IMappingExpression<D, S> mapping);

        /// <summary>
        /// Indicates in what order the mapping will be added.
        /// </summary>
        public abstract int Order { get; }
    }
}
