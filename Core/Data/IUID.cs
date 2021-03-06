﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data {
    /// <summary>
    /// All classes that represent a DB table
    /// and have a unique identifier must implement this interface.
    /// </summary>
    public interface IUID {

        /// <summary>
        /// The PRIMARY KEY constraint uniquely identifies each record in a database table.
        /// </summary>
        object Id { get; set; }
    }
}
