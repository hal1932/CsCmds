using Autodesk.Maya.OpenMaya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsCmds
{
    public static class MObjectExtensions
    {
        public static bool IsFilterd(this MObject obj, Func<MObject, bool> filter)
        {
            return (filter != null) ? filter(obj) : true;
        }
    }
}
