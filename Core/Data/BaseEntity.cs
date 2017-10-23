using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data {
    /// <summary>
    /// An abstract implementation of int IBaseEnity interface.
    /// </summary>
    public abstract class BaseEntity : IBaseEntity, IUID {

        /// <summary>
        /// The PRIMARY KEY constraint uniquely identifies each record in a database table.
        /// </summary>
        public object Id { get; set; }
    }
}
