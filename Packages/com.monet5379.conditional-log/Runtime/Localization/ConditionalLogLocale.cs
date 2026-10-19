using System;

namespace ConditionalLog
{
    /// <summary>
    /// Package UI string lookup (<see cref="ConditionalLogStrings"/>).
    /// </summary>
    public static class ConditionalLogLocale
    {
        public static string T(string key) => ConditionalLogStrings.Get(key);

        public static string T(string key, params object[] args)
        {
            var format = T(key);
            if (args == null || args.Length == 0)
                return format;
            try
            {
                return string.Format(format, args);
            }
            catch (FormatException)
            {
                return format;
            }
        }
    }
}
