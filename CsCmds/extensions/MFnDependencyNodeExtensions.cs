using Autodesk.Maya.OpenMaya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsCmds
{
    public static class MFnDependencyNodeExtensions
    {
        public static bool IsFilterd(this MFnDependencyNode fn, Func<MFnDependencyNode, bool> filter)
        {
            return (filter != null) ? filter(fn) : true;
        }
    }
}
