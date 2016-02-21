using Autodesk.Maya.OpenMaya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsCmds.Dag
{
    public class Shape : DagNode
    {
        internal Shape(MObject obj, MFnDagNode fn)
            : base(obj, fn)
        { }
    }
}
