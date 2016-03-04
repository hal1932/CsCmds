using Autodesk.Maya.OpenMaya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsCmds.extensions
{
    public static class MCommandResultExtensions
    {
        public static T GetResult<T>(this MCommandResult result)
        {
            var type = typeof(T);
            T value = default(T);

            if (type == typeof(int)) result.getResult((out int)Convert.ChangeType(out value, typeof(out int)));
            else if (type == typeof(double)) result.getResult(out value);

            return result;
        }

        private static void CopyTypedResult<T, U>(out T outValue, U value)
        {
            outValue = (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
