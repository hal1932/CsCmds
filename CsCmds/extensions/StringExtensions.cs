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
    }
}
