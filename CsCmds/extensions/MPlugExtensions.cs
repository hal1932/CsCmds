using Autodesk.Maya.OpenMaya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsCmds
{
    public static class MPlugExtensions
    {
        public static bool IsFilterd(this MPlug plug, Func<MPlug, bool> filter)
        {
            return (filter != null) ? filter(plug) : true;
        }
    }
}
