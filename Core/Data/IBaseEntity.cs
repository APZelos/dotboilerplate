using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data {
    /// <summary>
    /// All classes that represent a DB table
    /// must implement this interface.
    /// </summary>
    public interface IBaseEnity<T> : IComparable<IBaseEnity<T>> {

        /// <summary>
        /// The PRIMARY KEY constraint uniquely identifies each record in a database table.
        /// </summary>
        T Id { get; set; }
    }
}
