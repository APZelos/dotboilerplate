using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace Core.Infrastructure {
    /// <summary>
    /// Provides information about types in the current web application. 
    /// Optionally this class can look at all assemblies in the bin folder.
    /// </summary>
    public class WebAppTypeFinder : AppDomainTypeFinder {
        #region Fields and Properties
        /// <summary>
        /// Gets or sets whether assemblies in the bin folder of the web application should be specifically checked for being loaded on application load. 
        /// This is need in situations where plugins need to be loaded in the AppDomain after the application been reloaded.
        /// </summary>
        public bool EnsureBinFolderAssembliesLoaded { get; set; } = true;

        /// <summary>
        /// Indicates if the bin folder assemblies have been loaded.
        /// </summary>
        private bool _binFolderAssembliesLoaded;
        #endregion Fields and Properties

        #region Public Methods
        /// <summary>
        /// Gets a physical disk path of \Bin directory
        /// </summary>
        /// <returns>the physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
        public virtual string GetBinDirectory() {
            if (HostingEnvironment.IsHosted) {
                //hosted
                return HttpRuntime.BinDirectory;
            }

            //not hosted. For example, run either in unit tests
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// Gets all the assemblies.
        /// </summary>
        /// <returns>a list of assemblies.</returns>
        public override IList<Assembly> GetAssemblies() {
            if (this.EnsureBinFolderAssembliesLoaded && !_binFolderAssembliesLoaded) {
                _binFolderAssembliesLoaded = true;
                string binPath = GetBinDirectory();
                LoadMatchingAssemblies(binPath);
            }

            return base.GetAssemblies();
        }
        #endregion Public Methods
    }
}
