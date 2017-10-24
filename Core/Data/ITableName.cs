using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data {
    /// <summary>
    /// Every entity that will be fetched by a
    /// CachedByTableRepository must implement this interface.
    /// </summary>
    public interface ITableName {

        /// <summary>
        /// The name of the table that the entity
        /// that implements the interface represents.
        /// </summary>
        string TableName { get; }
    }
}
