using System;
using System.IO;

namespace CsCmds
{
    public static class StringExtensions
    {
        public static string AsNormalizedPath(this string value)
        {
            if (Path.DirectorySeparatorChar != '/')
            {
                return value.Replace(Path.DirectorySeparatorChar, '/');
            }
            return value;
        }

        public static bool IsFilterd(this string value, Func<string, bool> filter)
        {
            return (filter != null) ? filter(value) : true;
        }
    }
}
