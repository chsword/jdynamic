
namespace Chsword
{
    internal static class CExtender
    {
        /// <summary>
        /// Toes the specified obj.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static T To<T>(this object obj)
        {
            return (T)obj;
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static string GetContent(this string obj)
        {
            if (string.IsNullOrWhiteSpace(obj))
                return obj;

            if (obj[0] == '"' || obj[0] == '\'')
                return obj.Substring(1, obj.Length - 2);
            else
                return obj;
        }
    }
}
