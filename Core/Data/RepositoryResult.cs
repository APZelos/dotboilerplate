﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data {
    /// <summary>
    /// The result of a repository action.
    /// </summary>
    public class RepositoryResult<T> where T: IBaseEnity {
        /// <summary>
        /// Indicates if the action was successfull.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// A message returned from repository in case something went wrong.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The entity after the repository action (null if action was not successfull.)
        /// </summary>
        public T Entity { get; set; }

        /// <summary>
        /// Generic objet, placeholder for data that must be passed from repository to service.
        /// </summary>
        public object Obj { get; set; }
    }
}
