using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Core.Config {
    /// <summary>
    /// Handles the configuration settings
    /// in the web config.
    /// </summary>
    public class EngineConfig {
        #region Fields and Properties
        #endregion Fields and Properties

        #region Public Methods
        /// <summary>
        /// Creates a configuration section handler.
        /// </summary>
        /// <param name="parent">Parent object.</param>
        /// <param name="configContext">Configuration context object.</param>
        /// <param name="section">Section XML node.</param>
        /// <returns>the created section handler object.</returns>
        public object Create(object parent, object configContext, XmlNode section) {
            var config = new EngineConfig();

            return config;
        }
        #endregion Public Methods

        #region Private Methods
        /// <summary>
        /// Gets a string value from an xml node.
        /// </summary>
        /// <param name="node">The xml node that contains the value.</param>
        /// <param name="attrName">The values attribute name.</param>
        /// <returns>the value converted to a string.</returns>
        private string GetString(XmlNode node, string attrName) {
            return SetByXElement<string>(node, attrName, Convert.ToString);
        }

        /// <summary>
        /// Gets a bool value from an xml node.
        /// </summary>
        /// <param name="node">The xml node that contains the value.</param>
        /// <param name="attrName">The values attribute name.</param>
        /// <returns>the value converted to a boolean.</returns>
        private bool GetBool(XmlNode node, string attrName) {
            return SetByXElement<bool>(node, attrName, Convert.ToBoolean);
        }

        /// <summary>
        /// Gets a value from an xml node and converts it.
        /// </summary>
        /// <typeparam name="T">The type that the value will have after the convertion.</typeparam>
        /// <param name="node">The xml node that contains the value.</param>
        /// <param name="attrName">The values attribute name.</param>
        /// <param name="converter">The converter.</param>
        /// <returns>the converted value.</returns>
        private T SetByXElement<T>(XmlNode node, string attrName, Func<string, T> converter) {
            if (node == null || node.Attributes == null) return default(T);
            var attr = node.Attributes[attrName];
            if (attr == null) return default(T);
            var attrVal = attr.Value;
            return converter(attrVal);
        }
        #endregion Private Methods
    }
}