using System;
using System.Collections.Generic;
using System.Globalization;
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

        /// <summary>
        /// Capitalizes each word in the string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="currentCulture">If true uses the current culture otherwise the invariant culture. Default: false.</param>
        /// <returns>a string with each word capitalized.</returns>
        public static string ToTitleCase(this string str, bool currentCulture = false) {
            // If the string is empty or too short
            // returns the string intact.
            if (str.IsNullOrWhiteSpace() || str.Length < 2)
                return str;
            return currentCulture ?
                CultureInfo
                    .CurrentCulture
                    .TextInfo
                    .ToTitleCase(str) :
                CultureInfo
                    .InvariantCulture
                    .TextInfo
                    .ToTitleCase(str);
        }

        /// <summary>
        /// Capitalizes and joins each word in the string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="currentCulture">If true uses the current culture otherwise the invariant culture. Default: false.</param>
        /// <returns>a string with each word capitalized and joined.</returns>
        public static string ToCamelCase(this string str, bool currentCulture = false) {
            var result = str.ToTitleCase(currentCulture);
            return result
                .Replace(" ", "")
                .Replace("_", "")
                .Replace("-", "")
                .Replace(".", "");
        }

        /// <summary>
        /// Capitalizes, all except the first word, and joins each word in the string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="currentCulture">If true uses the current culture otherwise the invariant culture. Default: false.</param>
        /// <returns>a string with each word capitalized, except the first word, and joined.</returns>
        public static string ToPascalCase(this string str, bool currentCulture = false) {
            var result = str.ToCamelCase(currentCulture);
            return new StringBuilder()
                .Append(result.Substring(0, 1).ToLower())
                .Append(result.Substring(1))
                .ToString();
        }

        /// <summary>
        /// Removes the last char(s) of the string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length">The number of char that will be removed. Default: 1.</param>
        /// <returns></returns>
        public static string RemoveLast(this string str, int length = 1) => str.Substring(str.Length - length);

        /// <summary>
        /// Removes the first char(s) of the string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length">The number of char that will be removed. Default: 1.</param>
        /// <returns></returns>
        public static string RemoveFirst(this string str, int length = 1) => str.Substring(0, length);

        #endregion Strings
    }
}
