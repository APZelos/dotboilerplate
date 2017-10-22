using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data {
    /// <summary>
    /// An abstract implemention of int IBaseEnity interface.
    /// </summary>
    public abstract class BaseEntity : IBaseEnity, IUID<int> {

        /// <summary>
        /// The PRIMARY KEY constraint uniquely identifies each record in a database table.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// IComparable implementation.
        /// </summary>
        /// <param name="other">The BaseEntity that this one will be compared with.</param>
        /// <returns></returns>
        public int CompareTo(IUID<int> other) => this.Id - other.Id;
    }
}
