using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities {
    public static class Extensions {
        #region Strings

        /// <summary>
        /// Indicates whether the string is null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>true if the string is null or empty, or if consists exclusively of white-space characters.</returns>
        public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);

        /// <summary>
        /// Indicates whether the string is null or empty.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>true if the string is null or empty.</returns>
        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);

        /// <summary>
        /// Replaces the format item in the string with the string representation of a corresponding object in a specified array.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns></returns>
        public static string Format(this string str, params object[] args) => string.Format(str, args);

        #endregion Strings
    }
}
