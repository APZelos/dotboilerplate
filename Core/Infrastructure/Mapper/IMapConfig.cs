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
    public interface IMapConfig {

        /// <summary>
        /// Get the configurations.
        /// </summary>
        /// <returns>a mapping configuration mapping.</returns>
        Action<IMapperConfigurationExpression> GetConfiguration();

        /// <summary>
        /// Indicates in what order the mapping will be added.
        /// </summary>
        int Order { get; }
    }
}
