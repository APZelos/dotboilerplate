using Core.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Infrastructure {
    /// <summary>
    /// A class that finds types needed by looping assemblies in the 
    /// currently executing AppDomain. Only assemblies whose names matches
    /// certain patterns are investigated and an optional list of assemblies
    /// referenced by AssemblyNames are always investigated.
    /// </summary>
    public class AppDomainTypeFinder : ITypeFinder {
        #region Fields and Properties
        /// <summary>
        /// Indicates if the reflection erros should be thrown
        /// or should be consumed silently.
        /// </summary>
        private bool _ignoreReflectionErrors = true;

        /// <summary>
        /// Indicates if the AppDomain assemblies should be loaded.
        /// </summary>
        public bool LoadAppDomainAssemblies { get; set; } = true;

        /// <summary>
        /// The assemblies that matches this pattern
        /// will be skipped from loading.
        /// </summary>
        public string AssemblySkipLoadingPattern { get; set; } = "^System|^mscorlib|^Microsoft|^AjaxControlToolkit|^Antlr3|^Autofac|^AutoMapper|^Castle|^ComponentArt|^CppCodeProvider|^DotNetOpenAuth|^EntityFramework|^EPPlus|^FluentValidation|^ImageResizer|^itextsharp|^log4net|^MaxMind|^MbUnit|^MiniProfiler|^Mono.Math|^MvcContrib|^Newtonsoft|^NHibernate|^nunit|^Org.Mentalis|^PerlRegex|^QuickGraph|^Recaptcha|^Remotion|^RestSharp|^Rhino|^Telerik|^Iesi|^TestDriven|^TestFu|^UserAgentStringLibrary|^VJSharpCodeProvider|^WebActivator|^WebDev|^WebGrease";

        /// <summary>
        /// Only the assemblies that matches this pattern
        /// will be investigated
        /// </summary>
        public string AssemblyRestrictToLoadingPattern { get; set; } = ".*";

        /// <summary>
        /// The assemblies loaded at startup.
        /// </summary>
        public IList<string> AssemblyNames { get; set; } = new List<string>();

        /// <summary>
        /// The current AppDomain that will be searched for the types.
        /// </summary>
        public virtual AppDomain AppDomain => AppDomain.CurrentDomain;
        #endregion Fields and Properties

        #region Public Methods
        /// <summary>
        /// Finds all implementations of a type.
        /// </summary>
        /// <typeparam name="T">The type that the classes must implement.</typeparam>
        /// <param name="onlyConcreteClasses">If true collects only concrete classes, no abstracts. Default: true.</param>
        /// <returns>an enumerable of all types implementing the given type.</returns>
        public IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true)
            => FindClassesOfType(typeof(T), onlyConcreteClasses);

        /// <summary>
        /// Finds all implementations of a type.
        /// </summary>
        /// <param name="assignTypeFrom">The type that the classes must implement.</param>
        /// <param name="onlyConcreteClasses">If true collects only concrete classes, no abstracts. Default: true.</param>
        /// <returns>an enumerable of all types implementing the given type.</returns>
        public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true)
            => FindClassesOfType(assignTypeFrom, GetAssemblies(), onlyConcreteClasses);

        /// <summary>
        /// Finds all implementations of a type to a given list of assemblies.
        /// </summary>
        /// <typeparam name="T">The type that the classes must implement.</typeparam>
        /// <param name="assemblies">The assemblies that will be checked.</param>
        /// <param name="onlyConcreteClasses">If true collects only concrete classes, no abstracts. Default: true.</param>
        /// <returns>an enumerable of all types implementing the given type.</returns>
        public IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
            => FindClassesOfType(typeof(T), assemblies, onlyConcreteClasses);

        /// <summary>
        /// Finds all implementations of a type to a given list of assemblies.
        /// </summary>
        /// <param name="assignTypeFrom">The type that the classes must implement.</param>
        /// <param name="assemblies">The assemblies that will be checked.</param>
        /// <param name="onlyConcreteClasses">If true collects only concrete classes, no abstracts. Default: true.</param>
        /// <returns>an enumerable of all types implementing the given type.</returns>
        public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true) {
            var result = new List<Type>();
            try {
                foreach (var a in assemblies) {
                    Type[] types = null;

                    try {
                        types = a.GetTypes();
                    } catch {
                        //Entity Framework 6 doesn't allow getting types (throws an exception)
                        if (!_ignoreReflectionErrors) throw;
                    }

                    if (types.IsNull())
                        continue;

                    foreach (var t in types) {
                        if (!assignTypeFrom.IsAssignableFrom(t) &&
                            !(assignTypeFrom.IsGenericTypeDefinition && DoesTypeImplementOpenGeneric(t, assignTypeFrom)))
                            continue;

                        if (t.IsInterface) continue;

                        if (onlyConcreteClasses) {
                            if (!t.IsClass || t.IsAbstract)
                                continue;

                            result.Add(t);
                        } else result.Add(t);
                    }


                }
            } catch (ReflectionTypeLoadException ex) {
                var msg = string.Empty;
                foreach (var e in ex.LoaderExceptions)
                    msg += e.Message + Environment.NewLine;

                var fail = new Exception(msg, ex);
                Debug.WriteLine(fail.Message, fail);

                throw fail;
            }
            return result;
        }

        /// <summary>
        /// Gets all the assemblies.
        /// </summary>
        /// <returns>a list of assemblies.</returns>
        public virtual IList<Assembly> GetAssemblies() {
            var addedAssemblyNames = new List<string>();
            var assemblies = new List<Assembly>();

            if (LoadAppDomainAssemblies)
                AddAssembliesInAppDomain(addedAssemblyNames, assemblies);
            AddConfiguredAssemblies(addedAssemblyNames, assemblies);

            return assemblies;
        }

        /// <summary>
        /// Indicates if an assembly should be checked for add.
        /// </summary>
        /// <param name="assemblyFullName">The name of the assembly to check.</param>
        /// <returns>true if the assembly should be added.</returns>
        public virtual bool Matches(string assemblyFullName)
            => !Matches(assemblyFullName, AssemblySkipLoadingPattern)
                   && Matches(assemblyFullName, AssemblyRestrictToLoadingPattern);
        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Finds and adds all assemblies in the AppDomain that mathces the setted criteria.
        /// </summary>
        /// <param name="addedAssemblyNames">A list of that holds the names of the added assemblies.</param>
        /// <param name="assemblies">A list that holds the added assemblies.</param>
        private void AddAssembliesInAppDomain(List<string> addedAssemblyNames, List<Assembly> assemblies) {
            var assembliesToAdd = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(assembly => Matches(assembly.FullName));

            foreach (var assembly in assembliesToAdd) {
                if (assembly.FullName.IsIn(addedAssemblyNames))
                    continue;
                assemblies.Add(assembly);
                addedAssemblyNames.Add(assembly.FullName);
            }
        }

        /// <summary>
        /// Adds a specified list of assemblies.
        /// </summary>
        /// <param name="addedAssemblyNames">A list of that holds the names of the added assemblies.</param>
        /// <param name="assemblies">A list that holds the added assemblies.</param>
        protected virtual void AddConfiguredAssemblies(List<string> addedAssemblyNames, List<Assembly> assemblies) {
            foreach (string assemblyName in AssemblyNames) {
                var assembly = Assembly.Load(assemblyName);
                if (assembly.FullName.IsIn(addedAssemblyNames))
                    continue;
                assemblies.Add(assembly);
                addedAssemblyNames.Add(assembly.FullName);
            }
        }

        /// <summary>
        /// Indicates if the name of an assembly matches a given pattern.
        /// </summary>
        /// <param name="assemblyFullName">The name of the assembly to check.</param>
        /// <param name="pattern">The pattern that the assembly name would be checked against.</param>
        /// <returns>true if the name of the assembly matches the given pattern.</returns>
        protected virtual bool Matches(string assemblyFullName, string pattern)
            => Regex.IsMatch(assemblyFullName, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Loads all dlls in a specified directory that matches the setted criteria.
        /// </summary>
        /// <param name="directoryPath">The path of the directory that will be checked.</param>
        protected virtual void LoadMatchingAssemblies(string directoryPath) {
            if (!Directory.Exists(directoryPath)) return;

            var loadedAssemblyNames = GetAssemblies()
                .Select(assembly => assembly.FullName);

            foreach (var dll in Directory.GetFiles(directoryPath, "*.dll")) {
                try {
                    var assemblyName = AssemblyName.GetAssemblyName(dll);
                    if (!Matches(assemblyName.FullName) || assemblyName.FullName.IsIn(loadedAssemblyNames))
                        continue;
                    AppDomain.Load(assemblyName);
                } catch (BadImageFormatException ex) {
                    Trace.TraceError(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Indicates if a type is implementic a types generic definition.
        /// </summary>
        /// <param name="type">The type we want to check.</param>
        /// <param name="openGeneric">The types against its generic we will check the given type.</param>
        /// <returns>true if the type implements the generic definition.</returns>
        protected virtual bool DoesTypeImplementOpenGeneric(Type type, Type openGeneric) {
            try {
                var genericTypeDefinition = openGeneric.GetGenericTypeDefinition();
                foreach (var implementedInterface in type.FindInterfaces((objType, objCriteria) => true, null)) {
                    if (!implementedInterface.IsGenericType)
                        continue;

                    var isMatch = genericTypeDefinition.IsAssignableFrom(implementedInterface.GetGenericTypeDefinition());
                    return isMatch;
                }
                return false;
            } catch {
                return false;
            }
        }
        #endregion Private Methods
    }
}
