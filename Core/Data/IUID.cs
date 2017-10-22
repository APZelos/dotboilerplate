using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data {
    /// <summary>
    /// All classes that represent a DB table
    /// and have a unique identifier must implement this interface.
    /// </summary>
    /// <typeparam name="T">The type of unique identifier.</typeparam>
    public interface IUID<T> : IComparable<IUID<T>> {

        /// <summary>
        /// The PRIMARY KEY constraint uniquely identifies each record in a database table.
        /// </summary>
        T Id { get; set; }
    }
}
