using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Mapper {
    /// <summary>
    /// The interface for configuring mapping
    /// across dlls.
    /// </summary>
    /// <typeparam name="S">The type of the source object.</typeparam>
    /// <typeparam name="D">The type of the destination object.</typeparam>
    public interface IMapConfig<S, D> {

        /// <summary>
        /// Get the configurations.
        /// </summary>
        /// <returns>a mapping configuration mapping.</returns>
        Action<IMapperConfigurationExpression> GetConfiguration();

        /// <summary>
        /// Configures the mapping of the source object to the destination object.
        /// </summary>
        /// <param name="mapping">The mapping object.</param>
        void MapSourceToDestinationConfig(IMappingExpression<S, D> mapping);

        /// <summary>
        /// Configures the mapping of the destination object to the source object.
        /// </summary>
        /// <param name="mapping">The mapping object.</param>
        void MapDestinationToSourceConfig(IMappingExpression<D, S> mapping);

        /// <summary>
        /// Indicates in what order the mapping will be added.
        /// </summary>
        int Order { get; }
    }
}
