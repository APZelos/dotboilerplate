using System;
using System.Collections.Generic;
using System.Reflection;

namespace Core.Infrastructure {
    /// <summary>
    /// Gets all assemblies and find implementations
    /// of types across the assemblies.
    /// </summary>
    public interface ITypeFinder {
        /// <summary>
        /// Gets all the assemblies.
        /// </summary>
        /// <returns>a list of assemblies.</returns>
        IList<Assembly> GetAssemblies();

        /// <summary>
        /// Finds all implementations of a type.
        /// </summary>
        /// <param name="assignTypeFrom">The type that the classes must implement.</param>
        /// <param name="onlyConcreteClasses">If true collects only concrete classes, no abstracts. Default: true.</param>
        /// <returns>an enumerable of all types implementing the given type.</returns>
        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true);

        /// <summary>
        /// Finds all implementations of a type to a given list of assemblies.
        /// </summary>
        /// <param name="assignTypeFrom">The type that the classes must implement.</param>
        /// <param name="assemblies">The assemblies that will be checked.</param>
        /// <param name="onlyConcreteClasses">If true collects only concrete classes, no abstracts. Default: true.</param>
        /// <returns>an enumerable of all types implementing the given type.</returns>
        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

        /// <summary>
        /// Finds all implementations of a type.
        /// </summary>
        /// <typeparam name="T">The type that the classes must implement.</typeparam>
        /// <param name="onlyConcreteClasses">If true collects only concrete classes, no abstracts. Default: true.</param>
        /// <returns>an enumerable of all types implementing the given type.</returns>
        IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);

        /// <summary>
        /// Finds all implementations of a type to a given list of assemblies.
        /// </summary>
        /// <typeparam name="T">The type that the classes must implement.</typeparam>
        /// <param name="assemblies">The assemblies that will be checked.</param>
        /// <param name="onlyConcreteClasses">If true collects only concrete classes, no abstracts. Default: true.</param>
        /// <returns>an enumerable of all types implementing the given type.</returns>
        IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);
    }
}
