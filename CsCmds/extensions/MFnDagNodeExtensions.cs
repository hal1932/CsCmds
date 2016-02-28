using Autodesk.Maya.OpenMaya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsCmds
{
    public static class MFnDagNodeExtensions
    {
        public static bool IsFiltered(this MFnDagNode node, Func<MFnDagNode, bool> filter)
        {
            return (filter != null) ? filter(node) : true;
        }
    }
}
